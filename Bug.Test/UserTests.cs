using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker;

namespace UserTests
{
    public class UserTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Constructor_ThrowsExceptionWhenNameIsInvalid(string invalidName)
        {
            //Arrage, Act, and Assert
            Assert.Throws<ArgumentException>(() => new User(
                invalidName,
                "Test123",
                Role.GeneralUser
            ));
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Constructor_ThrowsExceptionWhenPasswordIsInvalid(string invalidPassword)
        {
            Assert.Throws<ArgumentException>(() => new User(
                "John",
                invalidPassword,
                Role.GeneralUser
            ));
        }
    }
}
