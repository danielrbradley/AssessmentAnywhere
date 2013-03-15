﻿namespace AssessmentAnywhere.Services.Repos
{
    using System.Collections.Generic;

    using AssessmentAnywhere.Services.Repos.Models;

    public class UserRepo
    {
        private static readonly Dictionary<string, User> Users = new Dictionary<string, User>() { { "test", new User("test", "123") } };

        public User Create(string username, string password)
        {
            var newUser = new User(username, password);
            Users.Add(username.ToLower(), newUser);
            return newUser;
        }

        public bool Exists(string username)
        {
            return Users.ContainsKey(username.ToLower());
        }

        public User Open(string username)
        {
            return Users[username.ToLower()];
        }

        public User OpenCurrentUser()
        {
            return Users[System.Threading.Thread.CurrentPrincipal.Identity.Name.ToLower()];
        }
    }
}