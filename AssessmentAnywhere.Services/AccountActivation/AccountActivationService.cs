namespace AssessmentAnywhere.Services.AccountActivation
{
    using System;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text;

    using AssessmentAnywhere.Services.Users;

    public class AccountActivationService : IAccountActivationService
    {
        private readonly IAccountActivationRepo accountActivationRepo;

        private readonly IUserRepo userRepo;

        private readonly Action<IAccountActivation> beginActivation;

        public AccountActivationService()
        {
            this.accountActivationRepo = new AccountActivationRepo();
            this.userRepo = new UserRepo();
        }

        public AccountActivationService(Action<IAccountActivation> beginActivation)
            : this(new AccountActivationRepo(), new UserRepo(), beginActivation)
        {
        }

        public AccountActivationService(IAccountActivationRepo accountActivationRepo, IUserRepo userRepo, Action<IAccountActivation> beginActivation)
        {
            this.accountActivationRepo = accountActivationRepo;
            this.userRepo = userRepo;
            this.beginActivation = beginActivation;
        }

        public void BeginActivation(IUser userAccount)
        {
            var accountActivation = this.accountActivationRepo.CreateOrReplace(userAccount.EmailAddress, userAccount.Username);
            this.beginActivation(accountActivation);
        }

        public CompleteActivationResult TryCompleteActivation(string emailAddress, string code)
        {
            if (!this.accountActivationRepo.Contains(emailAddress))
            {
                return CompleteActivationResult.NotFound;
            }

            var accountActivation = this.accountActivationRepo.Open(emailAddress);
            if (accountActivation.ActivationCode != code)
            {
                return CompleteActivationResult.IncorrectCode;
            }

            var completed = accountActivation.TryComplete();
            if (!completed)
            {
                return CompleteActivationResult.Expired;
            }

            this.userRepo.Open(accountActivation.Username).Activate();

            return CompleteActivationResult.Success;
        }
    }
}
