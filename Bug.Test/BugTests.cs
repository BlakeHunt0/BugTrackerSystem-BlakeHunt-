using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BugTracker;

namespace BugTest
{
    public class BugTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        //this will test every string above as a title, making three tests
        public void Constructor_ThrowsExceptionWhenTitleIsInvalid(string invalidTitle)
        {
            Assert.Throws<ArgumentException>(() => new Bug(
                invalidTitle,
                "Description"
            ));
        }
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            //Arrange and Act
            var bug = new Bug("Failed Login", "Description");
            //Assert
            Assert.Equal(1, bug.Id);
            Assert.Equal("Failed Login", bug.Title);
            Assert.Equal("Description", bug.Description);
            Assert.Equal(Status.Open, bug.Status);
        }

        //TODO: make this test work
        //how do i make this work if there is no data?
        //TODO: add in built bug data List<>
        [Fact]
        public void ReturnsExistingId()
        {

        }

        [Fact]
        public void CanAddAttachmentUrl()
        {
            var bug = new Bug("Getting odd menu", "I'm getting an unintended menu, here is a screenshot");
            bug.AttachmentUrl = "http://example.com/image.png";
            Assert.Equal("http://example.com/image.png", bug.AttachmentUrl);
        }
    }
}
