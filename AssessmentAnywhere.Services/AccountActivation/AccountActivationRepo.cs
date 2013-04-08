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

        public IAccountActivation CreateOrReplace(string username)
        {
            var expiry = this.currentTime() + this.activationWindow;
            var activationCode = this.activationCodeGenerator();
            var accountActivation = new AccountActivation(activationCode, username, expiry, false, this.currentTime);

            Activations.Remove(username);
            Activations.Add(username, accountActivation);
            return accountActivation;
        }

        public bool Contains(string username)
        {
            return Activations.ContainsKey(username);
        }

        public IAccountActivation Open(string username)
        {
            return Activations[username];
        }
    }
}
