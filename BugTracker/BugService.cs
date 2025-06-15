using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker
{
    public class BugService
    {
        private static int _nextId = 1;
        public static List<ReportedBug> _bugs = new List<ReportedBug>();

        /// <summary>
        /// Create the intitial list of bugs
        /// </summary>
        public void InitializeData()
        {
            if (_bugs.Count == 0)
            {
                _bugs.Clear();
                AddBug("Failed Login", "Users are unable to login using the correct credentials.", 3);
                AddBug("Missing Menus", "We are missing quiet a few menus requiered for the application to function properly.", 2);
                AddBug("Can't find bugs?", "The bug tracker is not showing any bugs, even though there are some on the List.", 1);
            }
        }

        /// <summary>
        /// Adds a new bug to the _bugs list.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="authorId"></param>
        public void AddBug(string title, string description, int authorId)
        {
            var newBug = new ReportedBug(title, description, authorId);
            newBug.Id = _nextId++;
            _bugs.Add(newBug);
        }
        /// <summary>
        /// Returns a list of all reported bugs.
        /// </summary>
        /// <returns></returns>
        public List<ReportedBug> GetAllBugs()
        {
            return _bugs.ToList();
        }
        /// <summary>
        /// Get a specific bug from _bugs by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public ReportedBug GetBugById(int id)
        {
            var bug = _bugs.FirstOrDefault(b => b.Id == id);
            if (bug != null)
            {
                return bug;
            }
            else
            {
                throw new ArgumentException($"Bug with ID {id} not found.");
            }
        }
        /// <summary>
        /// Immediately closes a bug regardless of its current status.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CloseBug(int id)
        {
            var bug = _bugs.FirstOrDefault(b => b.Id == id);
            if (bug == null)
            {
                return false;
            }
            if (bug.Status == Status.Closed)
            {
                return false;
            }
            bug.UpdateBugStatus(Status.Closed);
            bug.DateClosed = DateTime.Now;
            return true;
        }
        /// <summary>
        /// Permanently delets a bug from the _bugs list.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteBug(int id)
        {
            var bug = _bugs.FirstOrDefault(b => b.Id == id);
            if (bug != null)
            {
                _bugs.Remove(bug);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Progresses a bugs status to the next status.
        /// </summary>
        /// <param name="bugId"></param>
        public void NextStatus(int bugId)
        {
            var bug = _bugs.FirstOrDefault(b => b.Id == bugId);
            Status currentStatus =  bug.Status;

            if (currentStatus == Status.Open)
            {
                bug.Status = Status.InProgress;
            }
            else if (currentStatus == Status.InProgress)
            {
                bug.Status = Status.Pending;
            }
            else if (currentStatus == Status.Pending)
            {
                bug.Status = Status.Closed;
                bug.DateClosed = DateTime.Now;
            }
        }
        /// <summary>
        /// Assigns a severity level to a bug based on the provided severity integer between 1-4.
        /// </summary>
        /// <param name="bugId"></param>
        /// <param name="severity"></param>
        public void AssignSeverityToBug(int bugId, int severity)
        {
            var bug = _bugs.FirstOrDefault(b => b.Id == bugId);
            if (bug != null)
            {
                if (severity == 1)
                {
                    bug.Severity = SeverityLevel.Low;
                }
                if (severity == 2)
                {
                    bug.Severity = SeverityLevel.Medium;
                }
                if (severity == 3)
                {
                    bug.Severity = SeverityLevel.High;
                }
                if (severity == 4)
                {
                    bug.Severity = SeverityLevel.Critical;
                }
            }
        }
        /// <summary>
        /// Assigns a developer to a bug by its ID and the user's ID.
        /// </summary>
        /// <param name="bugId"></param>
        /// <param name="userId"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AssignDevToBug (int bugId, int userId)
        {
            UserService userService = new UserService();

            var bug = _bugs.FirstOrDefault(b => b.Id == bugId);
            User user = userService.GetUserById(userId);

            if (bug != null && user != null && user.UserRole == Role.Developer)
            {
                bug.AssignedTo = user.Name;
            }
            else
            {
                throw new ArgumentException($"Bug with ID {bugId} or User with ID {userId} not found.");
            }
        }
        /// <summary>
        /// Adds a comment to a bugs comment list.
        /// </summary>
        /// <param name="bugId"></param>
        /// <param name="authorId"></param>
        /// <param name="text"></param>
        public void AddCommentToBug(int bugId, int authorId, string text)
        {
            var bug = _bugs.FirstOrDefault(b => b.Id == bugId);
            if (bug != null)
            {
                Comment comment = new Comment(authorId, text);
                bug.Comments.Add(comment);
            }
        }
    }
}
