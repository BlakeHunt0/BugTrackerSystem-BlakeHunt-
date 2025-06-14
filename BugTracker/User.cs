using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker
{
    public class User
    {
        private static int _nextId = 1;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Role UserRole { get; set; } = Role.GeneralUser;

        public User(string username, string password, Role role = Role.GeneralUser)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username cannot be empty or white space.", nameof(username));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty or white space.", nameof(password));
            }
            Id = _nextId++;
            Name = username;
            Password = password;
            UserRole = role;
        }

        //TODO: allow users to create new accounts.
    }

    public enum Role
    {
        Admin,
        Developer,
        GeneralUser
    }
}
