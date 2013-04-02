namespace AssessmentAnywhere.Services.Users
{
    using System;
    using System.Net.Mail;

    internal class User : IUser
    {
        private readonly string username;

        private string password;

        private bool isEmailAddressValidated;

        public User(string username, string password, string emailAddress)
        {
            // Validate email address
            var asMailAddress = new MailAddress(emailAddress);

            this.username = username;
            this.password = password;
            this.EmailAddress = emailAddress;
            this.isEmailAddressValidated = false;
        }

        public string Username
        {
            get
            {
                return this.username;
            }
        }

        public string EmailAddress { get; private set; }

        public bool IsActive
        {
            get
            {
                return isEmailAddressValidated;
            }
        }

        public void SetEmailAddress(string updatedEmailAddress)
        {
            // Validate email address
            var asMailAddress = new MailAddress(updatedEmailAddress);

            this.EmailAddress = updatedEmailAddress;
        }

        public bool ValidatePassword(string passwordToTest)
        {
            return string.Equals(this.password, passwordToTest);
        }

        public void ChangePassword(string newPassword, string existingPassword)
        {
            if (!this.ValidatePassword(existingPassword))
            {
                throw new IncorrectPasswordException();
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new ArgumentNullException("newPassword", "Password must have a value.");
            }

            this.password = newPassword;
        }

        public void Activate()
        {
            this.isEmailAddressValidated = true;
        }
    }
}