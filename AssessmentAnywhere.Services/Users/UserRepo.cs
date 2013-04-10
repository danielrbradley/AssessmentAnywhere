namespace AssessmentAnywhere.Services.Users
{
    using System.Collections.Generic;
    using System.Linq;

    public class UserRepo : IUserRepo
    {
        private static readonly Dictionary<string, User> Users = new Dictionary<string, User> { { "test", new User("test", "123", "test@test.com") } };

        public IUser Create(string username, string password, string emailAddress)
        {
            var newUser = new User(username, password, emailAddress);
            if (EmailAddressExists(emailAddress))
            {
                throw new EmailAddressDuplicateException();
            }

            Users.Add(username.ToLower(), newUser);
            return newUser;
        }

        public bool EmailAddressExists(string emailAddress)
        {
            return Users.Any(u => u.Value.EmailAddress == emailAddress);
        }

        public bool Exists(string username)
        {
            return Users.ContainsKey(username.ToLower());
        }

        public IUser Open(string username)
        {
            return Users[username.ToLower()];
        }
    }
}
