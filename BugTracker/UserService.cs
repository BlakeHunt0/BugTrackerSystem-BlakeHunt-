using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker
{
    /// <summary>
    /// Various servie methods for the user class
    /// </summary>
    public class UserService
    {
        private static int _nextId = 1;

        /// <summary>
        /// List of initial data to test with.
        /// </summary>
        public static readonly List<User> _users = new List<User>
        {
            new User ( "admin", "admin123", Role.Admin ),
            new User ( "dev1", "dev123", Role.Developer ),
            new User ( "user1", "user123")
        };
        /// <summary>
        /// Add a user to the User List<>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void AddUser(string username, string password)
        {
            var newUser = new User(username, password);
            newUser.Id = _nextId++;
            _users.Add(newUser);
        }
        /// <summary>
        /// Get all entries from the User List<>
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            return _users.ToList();
        }
        /// <summary>
        /// Get a specific User from the User List<>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
        /// <summary>
        /// Find a User in the User List<> that has the username and password entered.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUserByCred(string username, string password)
        {
            User curruser = _users.FirstOrDefault(u => u.Name == username && u.Password == password);
            if (curruser != null)
            {
                return curruser;
            }
            return curruser;
        }
        /// <summary>
        /// Change a Users privlages to the input role level, 1 - 3.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleLevel"></param>
        /// <returns></returns>
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
