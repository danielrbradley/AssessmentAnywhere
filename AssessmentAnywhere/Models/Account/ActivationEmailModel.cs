namespace AssessmentAnywhere.Models.Account
{
    using System;

    using AssessmentAnywhere.Services.AccountActivation;

    public class ActivationEmailModel
    {
        private readonly string username;
        private readonly string emailAddress;
        private readonly string activationCode;
        private readonly DateTime expiry;
        private readonly ActivationState activationState;

        public ActivationEmailModel(string emailAddress, IAccountActivation accountActivation)
            : this(accountActivation.Username, emailAddress, accountActivation.ActivationCode, accountActivation.Expiry, accountActivation.State)
        {
        }

        public ActivationEmailModel(
            string username,
            string emailAddress,
            string activationCode,
            DateTime expiry,
            ActivationState activationState)
        {
            this.username = username;
            this.emailAddress = emailAddress;
            this.activationCode = activationCode;
            this.expiry = expiry;
            this.activationState = activationState;
        }

        public string Username
        {
            get
            {
                return this.username;
            }
        }

        public string EmailAddress
        {
            get
            {
                return this.emailAddress;
            }
        }

        public string ActivationCode
        {
            get
            {
                return this.activationCode;
            }
        }

        public DateTime Expiry
        {
            get
            {
                return this.expiry;
            }
        }

        public ActivationState ActivationState
        {
            get
            {
                return this.activationState;
            }
        }
    }
}
