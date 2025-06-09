using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker
{
    public class BugService
    {
        //TODO: I need to find a way to initialize this because currently if you report a bug before looking at the bugs list it will give your reported bugs the lowest bug IDs
        private static readonly List<Bug> _bugs = new List<Bug>
        {
            new Bug("Failed Login", "Users are unable to login using the correct credentials."),
            new Bug("Missing Menus", "We are missing quiet a few menus requiered for the application to function properly."),
            new Bug("Can't find bugs?", "The bug tracker is not showing any bugs, even though there are some on the List.")
        };
        public void AddBug(string title, string description)
        {
            var newBug = new Bug(title, description);
            _bugs.Add(newBug);
        }
        public List<Bug> GetAllBugs()
        {
            return _bugs.ToList();
        }
        public Bug GetBugById(int id)
        {
            return _bugs.FirstOrDefault(b => b.Id == id);
        }
        //TODO: bug.Status can be reverted to a previous version, try to fix this.
        public bool UpdateStatus(int id, Status newStatus)
        {
            var bug = _bugs.FirstOrDefault(b => b.Id == id);
            if (bug == null)
            {
                return false;
            }

            if (newStatus == Status.Closed)
            {
                bug.DateClosed = DateTime.Now;
            }

            bug.UpdateBugStatus(newStatus);
            return true;
        }
    }
}
