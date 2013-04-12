namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;

    using AssessmentAnywhere.Features;
    using AssessmentAnywhere.Models.Account;
    using AssessmentAnywhere.Services.AccountActivation;
    using AssessmentAnywhere.Services.Users;

    public class AccountController : Controller
    {
        private readonly IUserRepo userRepo;

        private readonly IAccountActivationService accountActivationService;

        private readonly Func<AccountMailerController> accountMailer;

        private readonly AccountFeatures features;

        public AccountController(IUserRepo userRepo, IAccountActivationService accountActivationService, Func<AccountMailerController> accountMailer, AccountFeatures features)
        {
            this.userRepo = userRepo;
            this.accountActivationService = accountActivationService;
            this.accountMailer = accountMailer;
            this.features = features;
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
                    if (!user.IsActive && this.features.RequireActivation)
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

                if (this.features.RequireActivation)
                {
                    // Send email validation message.
                    var activation = this.accountActivationService.BeginActivation(user);
                    this.accountMailer().ActivationEmail(user, activation).Deliver();
                    return this.RedirectToAction("AwaitingAccountActivation", new { username = model.Username });
                }

                user.Activate();
                FormsAuthentication.SetAuthCookie(model.Username, false);
                return this.RedirectToAction("Index", "Home");
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
