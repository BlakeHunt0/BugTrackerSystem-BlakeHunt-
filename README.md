BugTracker
-------------------------------

BugTracker is a small system meant to be used in small development settings to manage the bugs found among the team.

Setup
-------------------------------
1. Download the file.
2. Click into the file folder.
3. Right click on "BugTrackerSystem(BlakeHunt).sln".
4. Select "Open With -> VisualStudio".

At this point you change the function of the files to your liking.

Structure
-------------------------------
* Visual menus are created in Program.cs. these are the menus you will see while the code is running.

* The User and Bug classes setup the structure and rules for the User and Bug objects.

* UserService and BugService work with their own respective List<> objects for keeping track of and working with a number of users and bugs.

* There are a few tests in Test.ClassServiceTests.csproj.

Test Instructions
-------------------------------
Once you are in the code you can run the tests at any point by clicking on the "Test" button at the top left of your visual studio screen.
Then click the very first option that says, "Run all tests" this will bring up a tests menu that shows if created tests pass or fail.

Known Bugs / Future Ideas
-------------------------------
I believe that the main program is working as intended.

There was a setback on 06/13/25 where the only local git repo was destroyed. All of the thinking had been done, so there wansn't any planning needed, but the code needed to be remade.
This gave me the opportunity to create the repeat menus tab in the Program.cs file, which the original didn't have.

I want to greatly expand the test coverage, and get more comfortable making tests with my service classes.

I could also standardize and cleanup my menus. They are functional, but some look better than others.
