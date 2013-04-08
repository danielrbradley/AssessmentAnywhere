namespace AssessmentAnywhere.Models.Account
{
    public class AccountActivationModel
    {
        private readonly string username;

        public AccountActivationModel(string username)
        {
            this.username = username;
        }

        public string Username
        {
            get
            {
                return this.username;
            }
        }
    }
}