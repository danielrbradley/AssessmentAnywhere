namespace AssessmentAnywhere.Services.Users
{
    using System.Collections.Generic;

    public class UserRepo : IUserRepo
    {
        private static readonly Dictionary<string, User> Users = new Dictionary<string, User>() { { "test", new User("test", "123", "test@test.com") } };

        public IUser Create(string username, string password, string emailAddress)
        {
            var newUser = new User(username, password, emailAddress);
            Users.Add(username.ToLower(), newUser);
            return newUser;
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
