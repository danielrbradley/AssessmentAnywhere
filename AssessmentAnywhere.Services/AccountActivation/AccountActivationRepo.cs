namespace AssessmentAnywhere.Services.AccountActivation
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class AccountActivationRepo : IAccountActivationRepo
    {
        private readonly Func<string> activationCodeGenerator;

        private readonly TimeSpan activationWindow;

        private readonly Func<DateTime> currentTime;

        private static readonly Dictionary<string, IAccountActivation> Activations = new Dictionary<string, IAccountActivation>();

        public AccountActivationRepo()
            : this(() => Path.GetRandomFileName().Replace(".", string.Empty), TimeSpan.FromHours(1), () => DateTime.UtcNow)
        {
        }

        public AccountActivationRepo(Func<string> activationCodeGenerator, TimeSpan activationWindow, Func<DateTime> currentTime)
        {
            this.activationCodeGenerator = activationCodeGenerator;
            this.activationWindow = activationWindow;
            this.currentTime = currentTime;
        }

        public IAccountActivation CreateOrReplace(string emailAddress, string username)
        {
            var expiry = this.currentTime() + this.activationWindow;
            var activationCode = this.activationCodeGenerator();
            var accountActivation = new AccountActivation(emailAddress, activationCode, username, expiry, false, this.currentTime);

            Activations.Remove(emailAddress);
            Activations.Add(emailAddress, accountActivation);
            return accountActivation;
        }

        public bool Contains(string emailAddress)
        {
            return Activations.ContainsKey(emailAddress);
        }

        public IAccountActivation Open(string emailAddress)
        {
            return Activations[emailAddress];
        }
    }
}
