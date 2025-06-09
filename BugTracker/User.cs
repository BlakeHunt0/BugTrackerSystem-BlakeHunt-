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
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required Role UserRole { get; set; } = Role.GeneralUser;

        //TODO: allow users to create new accounts.
        //TODO: add comment system.
        //TODO: make this method bellow private by passing the data through
        public static List<User> users = new List<User>
        {
            new User { Id = 1, Name = "admin", Password = "admin123", UserRole = Role.Admin },
            new User { Id = 2, Name = "dev1", Password = "dev123", UserRole = Role.Developer },
            new User { Id = 3, Name = "user1", Password = "user123", UserRole = Role.GeneralUser }
        };
    }

    public enum Role
    {
        Admin,
        Developer,
        GeneralUser
    }
}
