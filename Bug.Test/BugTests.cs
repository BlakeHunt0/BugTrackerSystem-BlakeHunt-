using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BugTracker;

namespace BugTest
{
    public class BugTests
    {
        //describe the steps to the tests, this is a requirement
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Constructor_ThrowsExceptionWhenTitleIsInvalid(string invalidTitle)
        {
            Assert.Throws<ArgumentException>(() => new Bug(
                invalidTitle,
                "Description",
                3
            ));
        }
        //this is broken because I was messing with the IDs self incrementation, the id is comming back wrong
        //TODO: fix this
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            //Arrange and Act
            var bug = new Bug("Failed Login", "Description", 2);
            //Assert
            Assert.Equal(1, bug.Id);
            Assert.Equal("Failed Login", bug.Title);
            Assert.Equal("Description", bug.Description);
            Assert.Equal(Status.Open, bug.Status);
        }

        //TODO: make this test work
        //TODO: add in built bug data List<>
        [Fact]
        public void ReturnsExistingId()
        {

        }

        [Fact]
        public void CanAddAttachmentUrl()
        {
            var bug = new Bug("Getting odd menu", "I'm getting an unintended menu, here is a screenshot", 2);
            bug.AttachmentUrl = "http://example.com/image.png";
            Assert.Equal("http://example.com/image.png", bug.AttachmentUrl);
        }

        [Fact]
        public void ClosedBugUpdatesDateClosed()
        {
            //Arrange
            var bug = new Bug("Easy Bug", "I found a very obvious bug", 2);
            bug.UpdateBugStatus(Status.Closed);
            //Act
            bug.DateClosed = DateTime.Now;
            //Assert
            Assert.NotNull(bug.DateClosed);
            Assert.Equal(Status.Closed, bug.Status);
        }
    }
}
