using System;
using System.Net.NetworkInformation;

namespace BugTracker
{
    public class Program
    {
        /// <summary>
        /// Starts the Program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            BugService bugService = new BugService();
            UserService userService = new UserService();

            User user = Login(userService);
            ShowMenu(bugService, userService, user);
        }
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
        /// <summary>
        /// Show the main menu for the Bug Tracker application.
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="user"></param>
        private static void ShowMenu(BugService bugSer, UserService userSer, User user)
        {
            Console.Clear();

            Console.WriteLine(
                "\nMenu" +
                "\n==============================" +
                "\n1. Report Bug\n" +
                "2. View Bugs\n" +
                "3. Update Bug\n" +
                "4. Delete Bug\n" +
                "5. Exit\n"
            );

            if (user.UserRole == Role.Admin || user.UserRole == Role.Developer)
            {
                Console.WriteLine(
                    "\nDeveloper Menu" +
                    "\n==============================" +
                    "\n6. Assign Severity to Bug"
                );
                
                if (user.UserRole == Role.Admin)
                {
                    Console.WriteLine(
                        "\nAdmin Menu" +
                        "\n==============================" +
                        "\n7. View All Users" +
                        "\n8. Assign Developer to Bug" +
                        "\n9. Promote User To Role"
                    );
                }

            }

            string MenuChoice = Console.ReadLine();

            //general user menu options
            if (MenuChoice == "1")
            {
                Console.Clear();
                ReportBug(bugSer, userSer, user);
            }
            if (MenuChoice == "2")
            {
                Console.Clear();
                ViewBugs(bugSer, userSer, user);
            }
            if (MenuChoice == "3")
            {
                Console.Clear();
                UpdateBug(bugSer, user);
            }
            if (MenuChoice == "4")
            {
                Console.Clear();
                DeleteBug(bugSer, userSer, user);
            }

            if (MenuChoice == "5")
            {
               Environment.Exit(0);
            }

            //dev options
            if (MenuChoice == "6" && (user.UserRole == Role.Developer || user.UserRole == Role.Admin))
            {
                Console.Clear();
                AssignSeverity(bugSer, userSer, user);
            }

            //admin options
            if (MenuChoice == "7" && user.UserRole == Role.Admin)
            {
                Console.Clear();
                ViewAllUsers();
            }
            if (MenuChoice == "8" && user.UserRole == Role.Admin)
            {
                Console.Clear();
                AssignDeveloperToBug();
            }
            if (MenuChoice == "9" && user.UserRole == Role.Admin)
            {
                Console.Clear();
                PromoteUserToRole();
            }
        }

        //----------------MENU CHOICES---------------
        /// <summary>
        /// Report a new bug.
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="user"></param>
        private static void ReportBug(BugService bugSer, UserService userSer, User user)
        {
            Console.Clear();

            Console.WriteLine("Enter Title");
            string Title = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Enter Description");
            string Description = Console.ReadLine();

            int userId = user.Id;

            bugSer.AddBug(Title, Description, userId);

            Console.Clear();
            ShowMenu(bugSer, userSer, user);
        }
        /// <summary>
        /// Run a menu of diferent ways to view the bugs
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="user"></param>
        private static void ViewBugs(BugService bugSer, UserService userSer, User user)
        {
            Console.WriteLine(
                "1. View All Bugs\n" +
                "2. View Bug by ID\n"
            );

            string viewChoice = Console.ReadLine();

            if (viewChoice == "1")
            {
                PrintBugs(bugSer, userSer, user);

                Console.WriteLine("Enter bug ID to get bug details");

                int input;
                bool isValid = int.TryParse((Console.ReadLine()), out input);

                if (isValid)
                {
                    PrintBugDetails(bugSer, userSer, user, input);
                    CommentMenu(bugSer, userSer, user, input);
                }
                Console.ReadLine();
            }
            if (viewChoice == "2")
            {
                Console.Clear();
                Console.WriteLine("Enter Bug ID");

                string bugId = Console.ReadLine();

                Console.Clear();

                Bug lookedBug = bugSer.GetBugById(int.Parse(bugId));

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

                CommentMenu(bugSer, userSer, user, lookedBug.Id);
            }

            Console.Clear();
            ShowMenu(bugSer, userSer, user);
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
            ShowMenu(currBugSer, userSer, user);
        }
        private static void DeleteBug(BugService currBugSer, UserService userSer, User user)
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
            ShowMenu(currBugSer, userSer, user);
        }
        //----------------- Developer Menus -----------------
        public static void AssignSeverity(BugService bugSer, UserService userSer, User user)
        {
            Console.Clear();
            PrintBugs(bugSer, userSer, user);

            Console.WriteLine("\nWhich bug do you want to assign a severity level to?");

            int input;
            bool isValid = int.TryParse((Console.ReadLine()), out input);

            if (isValid)
            {
                Console.Clear();
                Console.WriteLine("What Severity do you want to assign to bug\n " + input + "?" +
                    "\n1. Low" +
                    "\n2. Medium" +
                    "\n3. High" +
                    "\n4. Critical"
                );

                int bugId = input;

                bool isValidSeverity = int.TryParse(Console.ReadLine(), out input);

                if (isValidSeverity && (input > 0 && input < 5))
                {
                    bugSer.AssignSeverityToBug(bugId, input);
                    Console.Clear();
                    Console.WriteLine("Severity level assigned successfully to bug " + bugId);
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("That is not a valid severity level");
                    Console.ReadLine();
                }
            }
            ShowMenu(bugSer, userSer, user);
        }

        //----------------- Admin Menus -----------------
        public static void ViewAllUsers(BugService bugSer, UserService userSer, User user)
        {
            ShowMenu(bugSer, userSer, user);
        }
        public static void AssignDeveloperToBug(BugService bugSer, UserService userSer, User user)
        {

        }
        public static void PromoteUserToRole(BugService bugSer, UserService userSer, User user)
        {

        }

        //----------------- Repeat Menus -----------------
        private static void PrintBugs(BugService bugSer, UserService userSer, User user)
        {
            Console.Clear();
            Console.WriteLine("Bugs:\n================================");
            foreach (var bug in bugSer.GetAllBugs())
            {
                Console.WriteLine(
                    bug.Id + ": " + bug.Title + "\n" +
                    bug.Description + "\n" +
                    "Uploaded by - " + bug.Author + "\n"
                );

                if (user.UserRole == Role.Admin || user.UserRole == Role.Developer)
                {
                    Console.WriteLine(
                        "\nStatus: " + bug.Status + 
                        "\nAssigned to: " + bug.AssignedTo
                    );
                }
            }
        }
        private static void PrintBugDetails(BugService bugser, UserService userSer, User user, int bugId)
        {
            Console.Clear();

            Bug bug = bugser.GetBugById(bugId);
            Console.WriteLine(
                    bug.Id + ": " + bug.Title + "\n" +
                    bug.Description + "\n" +
                    "Uploaded by - " + bug.Author + "\n"
            );
            if (user.UserRole == Role.Admin || user.UserRole == Role.Developer)
            {
                Console.WriteLine("Assigned to: " + bug.AssignedTo + "\n");
                Console.WriteLine("Severity Level: " + bug.Severity + "\n");
                Console.WriteLine("Status: " + bug.Status + "\n");
                if (bug.Status == Status.Closed)
                {
                    Console.WriteLine("Date Closed: " + bug.DateClosed.Value.ToString("g") + "\n");
                }
            }
            foreach (var comment in bug.Comments)
            {
                Console.WriteLine("Comment by " + userSer.GetUserById(comment.AuthorId).Name + ": " + comment.Text + "\n");
            }
        }
        private static void CommentMenu(BugService bugSer, UserService userSer, User user, int bugId)
        {
            Console.WriteLine("\n\n\n1. Add Comment \n");
            Console.WriteLine("2. Back");

            string input = Console.ReadLine();

            if (input == "1")
            {
                Console.WriteLine("Please Write Comment");
                string text = Console.ReadLine();
                bugSer.AddCommentToBug(bugId, user.Id, text);
            }
            else
            {
                ViewBugs(bugSer, userSer, user);
            }
        }
    }

}
