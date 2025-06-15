using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BugTracker
{
    public class ReportedBug
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public SeverityLevel Severity { get; set; }
        public Status Status { get; set; } = Status.Open;
        public DateTime UploadDate { get; set; } = DateTime.Now;

        //image attachment
        //This doesn't get implimented into the bug report system.
        public string? AttachmentUrl { get; set; }

        public DateTime? DateClosed { get; set; } = null;

        public int AuthorId { get; set; }
        public string Author { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public ReportedBug(string title, string description, int authorId)
        {
            UserService userService = new UserService();
            User user = userService.GetUserById(authorId);

            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Bug title cannot be empty or white space.", nameof(title));
            }
            Title = title;
            Description = description;
            AuthorId = authorId;
            Author = user.Name;
            Status = Status.Open;
            UploadDate = DateTime.Now;
        }

        public void UpdateBugStatus(Status newStatus)
        {
            if (newStatus == Status)
            {
                return;
            }

            Status = newStatus;
        }
    }

    public enum SeverityLevel
    {
        Low,
        Medium,
        High,
        Critical
    }
    public enum Status
    {
        Open,
        InProgress,
        Pending,
        Closed
    }

    public class Comment
    {
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public DateTime cmtDate { get; set; }

        public Comment(int authorId, string text)
        {
            AuthorId = authorId;
            Text = text;
            cmtDate = DateTime.Now;
        }
    }
}
