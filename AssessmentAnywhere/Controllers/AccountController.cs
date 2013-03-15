namespace AssessmentAnywhere.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;

    using AssessmentAnywhere.Models.Account;
    using AssessmentAnywhere.Services.Repos;

    public class AccountController : Controller
    {
        private readonly UserRepo userRepo;

        public AccountController()
            : this(new UserRepo())
        {
        }

        public AccountController(UserRepo userRepo)
        {
            this.userRepo = userRepo;
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
                if (this.userRepo.Exists(model.UserName))
                {
                    ModelState.AddModelError(
                        string.Empty, "User name already exists. Please enter a different user name.");
                }

                // Attempt to register the user
                this.userRepo.Create(model.UserName, model.Password);

                FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                return this.RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        // GET: /Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
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
                else
                {
                    ModelState.AddModelError(
                        string.Empty, "The current password is incorrect or the new password is invalid.");
                }
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
