using System;
using System.Net.NetworkInformation;

namespace BugTracker
{
    public class Program
    {
        static void Main(string[] args)
        {
            BugService bugService = new BugService();
            UserService userService = new UserService();

            User user = Login(userService);
            ShowMenu(bugService, user);
        }

        //TODO: add XML documentation to the methods and classes
        /// <summary>
        /// Asks for a username and password, 
        /// checks if that user exists, 
        /// then returns the user with matching credentials.
        /// </summary>
        /// <returns></returns>
        public static User Login(UserService userService)
        {
            User curUser = null;

            while (curUser == null)
            {
                Console.Clear();
                Console.WriteLine("Please enter your username: ");
                string inputname = Console.ReadLine();
                Console.WriteLine("Please enter your password: ");
                string inputpassword = Console.ReadLine();

                curUser = userService.GetUserByCred(inputname, inputpassword);

                if (curUser != null)
                {
                    Console.Clear();
                    Console.WriteLine($"Welcome {curUser.Name}!");
                    Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid username or password. Please try again.");
                    Console.ReadLine();
                    break;
                }
            }
            return curUser;
        }

        //----------------Main Menu----------------
        private static void ShowMenu(BugService currBugSer, User user)
        {
            Console.Clear();

            Console.WriteLine(
                "1. Report Bug\n" +
                "2. View Bugs\n" +
                "3. Update Bug\n" +
                "4. Delete Bug\n" +
                "5. Add Comment\n" +
                "6. Exit\n"
            );

            string MenuChoice = Console.ReadLine();

            if (MenuChoice == "1")
            {
                Console.Clear();
                ReportBug(currBugSer, user);
            }
            if (MenuChoice == "2")
            {
                Console.Clear();
                ViewBugs(currBugSer, user);
            }
            if (MenuChoice == "3")
            {
                Console.Clear();
                UpdateBug(currBugSer, user);
            }
            if (MenuChoice == "4")
            {
                Console.Clear();
                UpdateBug(currBugSer, user);
            }
            if (MenuChoice == "5")
            {
                Console.Clear();
                DeleteBug(currBugSer, user);
            }

            if (MenuChoice == "6")
            {
               Environment.Exit(0);
            }
        }

        //----------------MENU CHOICES---------------
        private static void ReportBug(BugService currBugSer, User user)
        {
            Console.Clear();

            Console.WriteLine("Enter Title");
            string Title = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Enter Description");
            string Description = Console.ReadLine();

            int userId = user.Id;

            currBugSer.AddBug(Title, Description, userId);

            Console.Clear();
            ShowMenu(currBugSer, user);
        }
        private static void ViewBugs(BugService currBugSer, User user)
        {
            Console.WriteLine(
                "1. View All Bugs\n" +
                "2. View Bug by ID\n"
            );

            string viewChoice = Console.ReadLine();

            if (viewChoice == "1")
            {
                Console.Clear();
                Console.WriteLine("Bugs:\n================================");

                foreach (var bug in currBugSer.GetAllBugs())
                {
                    Console.WriteLine(
                        bug.Id + ": " + bug.Title + "\n" + 
                        bug.Description + "\n" +
                        "Uploaded by - " + bug.Author + "\n"
                    );
                }

                Console.ReadLine();
                //TODO: allow users to pick a bug to view in detail
            }
            if (viewChoice == "2")
            {
                Console.Clear();
                Console.WriteLine("Enter Bug ID");

                string bugId = Console.ReadLine();

                Console.Clear();

                Bug lookedBug = currBugSer.GetBugById(int.Parse(bugId));

                Console.WriteLine(
                    lookedBug.Title + " : " +  lookedBug.Description + "\n");
                if (lookedBug.Author != null)
                {
                    Console.WriteLine(lookedBug.Author + "\n");
                }
                foreach (var comment in lookedBug.Comments)
                {
                    Console.WriteLine(comment + "\n");
                }

                Console.WriteLine("\n\n\n1. Add Comment \n");
                Console.WriteLine("2. Back");

                string input = Console.ReadLine();

                if(input == "1")
                {
                    Console.WriteLine("Please Write Comment");
                    string Text = Console.ReadLine();
                    Comment newComment = new Comment(user.Id, Text);
                    //comments should also have bugId
                    //TODO: add this comment to the bug that it belongs to
                }
                else
                {
                    ViewBugs(currBugSer, user);
                }
            }

            Console.Clear();
            ShowMenu(currBugSer, user);
        }
        private static void UpdateBug(BugService currBugSer, User user)
        {
            if (user.UserRole == Role.Admin || user.UserRole == Role.Developer)
            {
                Console.WriteLine("What is the ID of the bug you are trying to change");

                int bugId = int.Parse(Console.ReadLine());

                Console.Clear();
                Console.WriteLine("What would you like to do to bug:    " + bugId + "?");
                Console.WriteLine(
                    "\n" +
                    "Close Bug\n" +
                    "Advance Bug Status\n"
                );

                Console.ReadLine();

                Bug bug = currBugSer.GetBugById(bugId);
                int input = int.Parse(Console.ReadLine());

                if (input == 1)
                {
                    bug.Status = Status.Closed;
                }
                if (input == 2)
                {
                    currBugSer.NextStatus(bugId);
                }
            }
            else
            {
                Console.WriteLine("Your Bugs: \n");

                foreach (var bug in currBugSer.GetAllBugs().Where(b => b.AuthorId == user.Id))
                {
                    Console.WriteLine(
                        bug.Id + ": " + bug.Title + "\n" +
                        bug.Description + "\n" +
                        "Uploaded by - " + user.Name + "\n"
                    );
                }

                Console.WriteLine("Which bug would you like to edit");
                int bugId = int.Parse(Console.ReadLine());

                Bug bugToEdit = currBugSer.GetBugById(bugId);

                Console.WriteLine("\nWhat would you like to do\n");
                Console.WriteLine(
                    "1. Edit Bug Title\n" +
                    "2. Edit Bug Description\n"
                );

                int input = int.Parse(Console.ReadLine());

                if (input == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Input new title: \n");

                    string title = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        bugToEdit.Title = title;
                        Console.WriteLine("Title updated successfully");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Title cannot be empty or whitespace");
                        Console.ReadLine();
                    }
                }
                if (input == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Input new description");

                    string description = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        bugToEdit.Description = description;
                        Console.WriteLine("Description updated successfully");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Description cannot be empty or whitespace");
                        Console.ReadLine();
                    }
                }
            }
        }
        private static void DeleteBug(BugService currBugSer, User user)
        {
            if (user.UserRole == Role.Admin || user.UserRole == Role.Developer)
            {
                foreach (var bug in currBugSer.GetAllBugs())
                {
                    Console.WriteLine(
                        bug.Id + ": " + bug.Title + "\n" +
                        bug.Description + "\n" +
                        "Uploaded by - " + user.Name + "\n"
                    );
                }

                Console.WriteLine("What bug.id would you like to delete");
                int bugId = int.Parse(Console.ReadLine());

                Bug bugToDelete = currBugSer.GetBugById(bugId);
                if (bugToDelete != null)
                {
                    currBugSer.GetAllBugs().Remove(bugToDelete);
                    Console.WriteLine("Bug deleted successfully");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("This Bug does not exist");
                    Console.ReadLine();
                }
            }
            if (user.UserRole == Role.GeneralUser)
            {
                Console.WriteLine("Your Bugs: \n");
                foreach (var bug in currBugSer.GetAllBugs().Where(Bug => Bug.AuthorId == user.Id))
                {
                    Console.WriteLine(
                        "\n-------------------------\n" +
                        bug.Id + ": " + bug.Title + ": \n" + 
                        bug.Description + 
                        "\n-------------------------\n"
                    );
                    Console.WriteLine("Which bug would you like to delete?");

                    int bugId = int.Parse(Console.ReadLine());

                    Bug bugToDelete = currBugSer.GetBugById(bugId);
                    if (bugToDelete != null)
                    {
                        currBugSer.GetAllBugs().Remove(bugToDelete);
                        Console.WriteLine("Bug deleted successfully");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("This Bug does not exist");
                        Console.ReadLine();
                    }
                }
            }

            Console.Clear();
            ShowMenu(currBugSer, user);
        }
    }

}
