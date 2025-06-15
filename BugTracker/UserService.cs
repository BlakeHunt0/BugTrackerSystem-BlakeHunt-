using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker
{
    public class UserService
    {
        private static int _nextId = 1;

        public static readonly List<User> _users = new List<User>
        {
            new User ( "admin", "admin123", Role.Admin ),
            new User ( "dev1", "dev123", Role.Developer ),
            new User ( "user1", "user123")
        };
        public void AddUser(string username, string password)
        {
            var newUser = new User(username, password);
            newUser.Id = _nextId++;
            _users.Add(newUser);
        }

        public List<User> GetAllUsers()
        {
            return _users.ToList();
        }
        public User GetUserById(int id)
        {
            User user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new ArgumentException($"User with ID {id} not found.");
            }
        }
        public User GetUserByCred(string username, string password)
        {
            User curruser = _users.FirstOrDefault(u => u.Name == username && u.Password == password);
            if (curruser != null)
            {
                return curruser;
            }
            return curruser;
        }

        public bool PromoteUser(int id, int roleLevel)
        {
            User user = GetUserById(id);
            if (user == null)
            {
                return false;
            }
            else
            {
                if (roleLevel == 1)
                {
                    user.UserRole = Role.GeneralUser;
                    return true;
                }
                else if (roleLevel == 2)
                {
                    user.UserRole = Role.Developer;
                    return true;
                }
                else if (roleLevel == 3)
                {
                    user.UserRole = Role.Admin;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
