namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;

    public class User
    {
        private readonly string username;
        private string password;

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string Username
        {
            get
            {
                return this.username;
            }
        }

        public bool ValidatePassword(string passwordToTest)
        {
            return string.Equals(this.password, passwordToTest);
        }

        public void ChangePassword(string newPassword, string existingPassword)
        {
            if (!ValidatePassword(existingPassword))
            {
                throw new IncorrectPasswordException();
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new ArgumentNullException("newPassword", "Password must have a value.");
            }

            this.password = newPassword;
        }
    }

    public class IncorrectPasswordException : Exception
    {
    }
}