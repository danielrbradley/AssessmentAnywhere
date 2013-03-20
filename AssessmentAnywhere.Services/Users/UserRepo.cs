namespace AssessmentAnywhere.Services.Users
{
    using System.Collections.Generic;

    public class UserRepo : IUserRepo
    {
        private static readonly Dictionary<string, User> Users = new Dictionary<string, User>() { { "test", new User("test", "123") } };

        public IUser Create(string username, string password)
        {
            var newUser = new User(username, password);
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

        public IUser OpenCurrentUser()
        {
            return Users[System.Threading.Thread.CurrentPrincipal.Identity.Name.ToLower()];
        }
    }
}
