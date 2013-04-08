namespace AssessmentAnywhere.Models.Account
{
    using System;

    using AssessmentAnywhere.Services.AccountActivation;

    public class ActivationEmailModel
    {
        private readonly string baseUrl;
        private readonly string username;
        private readonly string emailAddress;
        private readonly string activationCode;
        private readonly DateTime expiry;
        private readonly ActivationState activationState;

        public ActivationEmailModel(string baseUrl, string emailAddress, IAccountActivation accountActivation)
            : this(baseUrl, accountActivation.Username, emailAddress, accountActivation.ActivationCode, accountActivation.Expiry, accountActivation.State)
        {
        }

        public ActivationEmailModel(
            string baseUrl,
            string username,
            string emailAddress,
            string activationCode,
            DateTime expiry,
            ActivationState activationState)
        {
            this.baseUrl = baseUrl;
            this.username = username;
            this.emailAddress = emailAddress;
            this.activationCode = activationCode;
            this.expiry = expiry;
            this.activationState = activationState;
        }

        public string BaseUrl
        {
            get
            {
                return this.baseUrl;
            }
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
