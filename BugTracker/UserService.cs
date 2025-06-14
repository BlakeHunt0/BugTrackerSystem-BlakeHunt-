using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker
{
    public class UserService
    {
        private static readonly List<User> _users = new List<User>
        {
            new User ( "admin", "admin123", Role.Admin ),
            new User ( "dev1", "dev123", Role.Developer ),
            new User ( "user1", "user123")
        };

        //TODO: allow only admins to change the roles of users.
        public void AddUser(string username, string password)
        {
            var newUser = new User(username, password);
        }

        public List<User> GetAllUsers()
        {
            return _users.ToList();
        }
        public User GetUserById(int id)
        {
            User user = _users.FirstOrDefault(u => u.Id == id);
            return user;
        }
        public User GetUserByCred(string username, string password)
        {
            //this is throwing a whitespace exception when i type in the correct credentials
            User curruser = _users.FirstOrDefault(u => u.Name == username && u.Password == password);
            return curruser;
        }
    }
}
