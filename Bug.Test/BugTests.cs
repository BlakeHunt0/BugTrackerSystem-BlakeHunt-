using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BugTracker;

namespace BugTest
{
    public class BugTests
    {
        /// <summary>
        /// Tests that the constructor throws an exception when the title is null, empty, or whitespace.
        /// </summary>
        /// <param name="invalidTitle"></param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Constructor_ThrowsExceptionWhenTitleIsInvalid(string invalidTitle)
        {
            //creates a bug
            //puts bad data in the bug
            //and asserts that if the title is invalid, it throws an exception
            //Arrage, Act, and Assert
            Assert.Throws<ArgumentException>(() => new ReportedBug(
                invalidTitle,
                "Description",
                3
            ));
        }
        /// <summary>
        /// Tests that the constructor sets the properties correctly when valid data is provided.
        /// </summary>
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            var bug = new ReportedBug("Failed Login", "Description", 2);
            Assert.Equal("Failed Login", bug.Title);
            Assert.Equal("Description", bug.Description);
            Assert.Equal(Status.Open, bug.Status);
        }
        /// <summary>
        /// Tests if we can add an imageurl
        /// </summary>
        [Fact]
        public void Constructor_CanAddAttachmentUrl()
        {
            //Arrange
            var bug = new ReportedBug("Getting odd menu", "I'm getting an unintended menu, here is a screenshot", 2);
            //Act
            bug.AttachmentUrl = "http://example.com/image.png";
            //Assert
            Assert.Equal("http://example.com/image.png", bug.AttachmentUrl);
        }
        /// <summary>
        /// Tests to make sure that closed bugs get a date and are closed
        /// </summary>
        [Fact]
        public void CloseBug_UpdatesDateClosed()
        {
            //Arrange
            var bug = new ReportedBug("Easy Bug", "I found a very obvious bug", 2);
            bug.UpdateBugStatus(Status.Closed);
            //Act
            bug.DateClosed = DateTime.Now;
            //Assert
            Assert.NotNull(bug.DateClosed);
            Assert.Equal(Status.Closed, bug.Status);
        }
    }

    public class BugServiceTests
    {
        //Arrange
        BugService bugService = new BugService();
        [Fact]
        public void GetAllBugs_ReturnsAllInitialBugs()
        {
            //Arrange
            BugService initTestService = new BugService();
            initTestService.InitializeData();
            List<ReportedBug> initList = bugService.GetAllBugs();
            //Assert
            Assert.Equal(3, initList.Count);
        }
        [Fact]
        public void GetAllBugs_ReturnsCorrestNumberOfBugs()
        {
            //Act and Assert
            Assert.True(bugService.GetAllBugs().Count() == BugService._bugs.Count);
        }
        [Fact]
        public void AddBug_AddsToList()
        {
            //Arrange
            //InitializeData();
            //there was a data initialization here. but it carries over from other tests creating duplicate data
            //Act
            bugService.AddBug("Test Bug", "This bug tests the AddBug service", 1);
            //Assert
            Assert.Equal("Test Bug", bugService.GetBugById(4).Title);
            Assert.Equal("This bug tests the AddBug service", bugService.GetBugById(4).Description);
            Assert.Equal("admin", bugService.GetBugById(4).Author);
            Assert.Equal(Status.Open, bugService.GetBugById(4).Status);
        }
        [Fact]
        public void GetBugById_ReturnsExistingId()
        {
            //Arrange
            bugService.AddBug("Test Bug", "This bug tests the AddBug service", 1);
            //Act
            var bug = bugService.GetBugById(1);
            //Assert
            Assert.True(bug != null);
        }

    }
}
