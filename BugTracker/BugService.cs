using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker
{
    public class BugService
    {
        private static readonly List<Bug> _bugs = new List<Bug>
        {
            new Bug("Failed Login", "Users are unable to login using the correct credentials.", 3),
            new Bug("Missing Menus", "We are missing quiet a few menus requiered for the application to function properly.", 2),
            new Bug("Can't find bugs?", "The bug tracker is not showing any bugs, even though there are some on the List.", 1)
        };
        public void AddBug(string title, string description, int authorId)
        {
            var newBug = new Bug(title, description, authorId);
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
        public void NextStatus(int bugId)
        {
            Bug bug = GetBugById(bugId);
            Status currentStatus = bug.Status;

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
        public void AssignSeverityToBug(int bugId, int severity)
        {
            Bug bug = GetBugById(bugId);
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
        public void AddCommentToBug(int bugId, int authorId, string text)
        {
            Bug bug = GetBugById(bugId);
            if (bug != null)
            {
                Comment comment = new Comment(authorId, text);
                bug.Comments.Add(comment);
            }
        }
    }
}
