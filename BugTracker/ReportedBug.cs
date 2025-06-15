using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BugTracker
{
    /// <summary>
    /// Class for keeping track of the reported bugs in the application.
    /// </summary>
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

        /// <summary>
        /// Constructor to create bugs with valid credentials.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="authorId"></param>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// updates a bugs status
        /// </summary>
        /// <param name="newStatus"></param>
        public void UpdateBugStatus(Status newStatus)
        {
            if (newStatus == Status)
            {
                return;
            }

            Status = newStatus;
        }
    }
    /// <summary>
    /// enum to assign a severity Level to a bug.
    /// </summary>
    public enum SeverityLevel
    {
        Low,
        Medium,
        High,
        Critical
    }
    /// <summary>
    /// enum to assign a status to a bug.
    /// </summary>
    public enum Status
    {
        Open,
        InProgress,
        Pending,
        Closed
    }

    /// <summary>
    /// Class that saves commentor ID, and comment text to be saved in a bugs comment list for later viewing.
    /// </summary>
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
