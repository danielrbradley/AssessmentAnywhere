namespace AssessmentAnywhere.Controllers
{
    using ActionMailer.Net.Mvc;

    using AssessmentAnywhere.Models.Account;
    using AssessmentAnywhere.Services.AccountActivation;
    using AssessmentAnywhere.Services.Users;

    public class AccountMailerController : MailerBase
    {
        public EmailResult ActivationEmail(IUser user, IAccountActivation accountActivation)
        {
            this.To.Add(user.EmailAddress);
            this.From = "accounts@assessment-anywhere.com";
            this.Subject = "Activate your assessment anywhere account";

            var model = new ActivationEmailModel(user.EmailAddress, accountActivation);

            return this.Email("ActivationEmail", model);
        }
    }
}
