namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.IO;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Security;

    using AssessmentAnywhere.Models.Account;
    using AssessmentAnywhere.Services.AccountActivation;
    using AssessmentAnywhere.Services.Users;

    public class AccountController : Controller
    {
        private readonly UserRepo userRepo;

        private readonly IAccountActivationService accountActivationService;

        public AccountController()
        {
            this.userRepo = new UserRepo();
            this.accountActivationService = new AccountActivationService(this.OnBeginActivation);
        }

        private void OnBeginActivation(IUser user, IAccountActivation accountActivation)
        {
            var smtp = new SmtpClient();

            var baseUrl = Request.Url == null
                              ? "http://assessment-anywhere.com"
                              : Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Authority;

            var model = new ActivationEmailModel(baseUrl, user.EmailAddress, accountActivation);

            var htmlBody = this.RenderRazorViewToString("ActivationEmail", model);
            var textBody = this.RenderRazorViewToString("ActivationEmailTextView", model);

            var message = new MailMessage
                              {
                                  From = new MailAddress("accounts@assessment-anywhere.com"),
                                  To = { new MailAddress(user.EmailAddress) },
                                  Subject = "Activate your assessment anywhere account",
                                  Body = htmlBody,
                                  IsBodyHtml = true,
                                  BodyEncoding = Encoding.UTF8,
                                  BodyTransferEncoding = TransferEncoding.QuotedPrintable,
                                  AlternateViews =
                                      {
                                          AlternateView.CreateAlternateViewFromString(
                                              textBody, Encoding.UTF8, MediaTypeNames.Text.Plain)
                                      },
                              };
            smtp.Send(message);
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            this.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(this.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public AccountController(UserRepo userRepo, IAccountActivationService accountActivationService)
        {
            this.userRepo = userRepo;
            this.accountActivationService = accountActivationService;
        }

        // GET: /Account/LogOn
        public ActionResult SignIn()
        {
            return this.View();
        }

        // POST: /Account/LogOn
        [HttpPost]
        public ActionResult SignIn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (this.userRepo.Exists(model.UserName))
                {
                    var user = this.userRepo.Open(model.UserName);
                    if (!user.IsActive)
                    {
                        return this.RedirectToAction("AwaitingAccountActivation", new { username = model.UserName });
                    }

                    if (user.ValidatePassword(model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return this.Redirect(returnUrl);
                        }

                        return this.RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        // GET: /Account/LogOff
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return this.RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        public ActionResult Register()
        {
            return this.View();
        }

        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (this.userRepo.Exists(model.Username))
                {
                    ModelState.AddModelError(
                        string.Empty, "User name already exists. Please enter a different user name.");
                }

                // Attempt to register the user
                var user = this.userRepo.Create(model.Username, model.Password, model.EmailAddress);

                // Send email validation message.
                this.accountActivationService.BeginActivation(user);

                return this.RedirectToAction("AwaitingAccountActivation", new { username = model.Username });
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        [HttpGet]
        public ActionResult AwaitingAccountActivation(string username)
        {
            var model = new AccountActivationModel(username);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult ResendActivationEmail(string username)
        {
            var user = this.userRepo.Open(username);

            if (user.IsActive)
            {
                var model = new AccountActivationModel(username);
                return this.View("AlreadActivated", model);
            }

            this.accountActivationService.BeginActivation(user);

            return this.RedirectToAction("AwaitingAccountActivation", new { username });
        }

        public ActionResult ActivateAccount(string username, string code)
        {
            var result = this.accountActivationService.TryCompleteActivation(username, code);

            switch (result)
            {
                case CompleteActivationResult.Success:
                    return this.View("AccountActivated");
                case CompleteActivationResult.NotFound:
                    ModelState.AddModelError(string.Empty, "Your account activation session cannot be found.");
                    break;
                case CompleteActivationResult.IncorrectCode:
                    ModelState.AddModelError(string.Empty, "Your account activation code was incorrect.");
                    break;
                case CompleteActivationResult.Expired:
                    ModelState.AddModelError(string.Empty, "Your account activation session has expired.");
                    break;
            }

            var model = new AccountActivationModel(username);
            return this.View("AccountActivationFailed", model);
        }

        // GET: /Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return this.View();
        }

        // POST: /Account/ChangePassword
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    var user = this.userRepo.OpenCurrentUser();
                    user.ChangePassword(model.NewPassword, model.OldPassword);
                    changePasswordSucceeded = true;
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return this.RedirectToAction("ChangePasswordSuccess");
                }

                this.ModelState.AddModelError(
                    string.Empty, "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        // GET: /Account/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return this.View();
        }
    }
}
