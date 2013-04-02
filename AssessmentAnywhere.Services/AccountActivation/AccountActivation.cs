namespace AssessmentAnywhere.Services.AccountActivation
{
    using System;

    internal class AccountActivation : IAccountActivation
    {
        private readonly string emailAddress;

        private readonly string activationCode;

        private readonly string username;

        private readonly DateTime expiry;

        private bool completed;

        private readonly Func<DateTime> currentTimeProvider;

        public AccountActivation(
            string emailAddress,
            string activationCode,
            string username,
            DateTime expiry,
            bool completed,
            Func<DateTime> currentTimeProvider)
        {
            this.emailAddress = emailAddress;
            this.activationCode = activationCode;
            this.username = username;
            this.expiry = expiry;
            this.completed = completed;
            this.currentTimeProvider = currentTimeProvider;
        }

        public string EmailAddress
        {
            get
            {
                return emailAddress;
            }
        }

        public string ActivationCode
        {
            get
            {
                return this.activationCode;
            }
        }

        public string Username
        {
            get
            {
                return this.username;
            }
        }

        public DateTime Expiry
        {
            get
            {
                return this.expiry;
            }
        }

        public ActivationState State
        {
            get
            {
                if (this.completed)
                {
                    return ActivationState.Complete;
                }

                if (HasExpired)
                {
                    return ActivationState.Expired;
                }

                return ActivationState.Pending;
            }
        }

        private bool HasExpired
        {
            get
            {
                return this.expiry < this.currentTimeProvider();
            }
        }

        public bool TryComplete()
        {
            if (completed)
            {
                return true;
            }

            if (HasExpired)
            {
                return false;
            }

            this.completed = true;
            return true;
        }
    }
}