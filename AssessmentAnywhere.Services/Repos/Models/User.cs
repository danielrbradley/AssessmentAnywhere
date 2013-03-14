namespace AssessmentAnywhere.Services.Repos.Models
{
    public class User
    {
        private readonly string username;
        private readonly string password;

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

        public bool CheckPassword(string passwordToTest)
        {
            return string.Equals(this.password, passwordToTest);
        }
    }
}