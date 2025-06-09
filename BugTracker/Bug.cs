using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BugTracker
{
    public class Bug
    {
        private static int _nextId = 1;

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public SeverityLevel Severity { get; set; }
        public Status Status { get; set; } = Status.Open;
        public DateTime UploadDate { get; set; } = DateTime.Now;

        //image attachment
        public string? AttachmentUrl { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public Bug(string title, string description)
        {
            //I don't know if both parameters are needed
            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Bug title cannot be empty or white space.", nameof(title));
            }

            //this goes 123045678... zero being the first bug reported through the app, following bugs are given the correct IDs
            Id = _nextId++;
            Title = title;
            Description = description;
            Status = Status.Open;
            UploadDate = DateTime.Now;
            //TODO: get the reporters account username
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
        public string Author { get; set; }
        public string Data { get; set; }
        public DateTime cmtDate { get; set; }
    }
}
