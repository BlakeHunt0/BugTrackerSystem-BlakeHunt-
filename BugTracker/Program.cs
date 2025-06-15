using System;
using System.Net.NetworkInformation;

namespace BugTracker
{
    public class Program
    {
        /// <summary>
        /// Starts the Program.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            BugService bugService = new BugService();
            UserService userService = new UserService();

            //initialize the bug and user lists
            bugService.InitializeData();
            userService.GetAllUsers();

            User user = Login(bugService,userService);
        }

        /// <summary>
        /// Asks for a username and password, 
        /// checks if that user exists, 
        /// then returns the user with matching credentials.
        /// </summary>
        /// <returns></returns>
        public static User Login(BugService bugService, UserService userService)
        {
            User curUser = null;

            while (curUser == null)
            {
                //TODO: randomly this script will not work and I will have to enter the credentials multiple times before I login.
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
                    ShowMenu(bugService, userService, curUser);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid username or password. Please try again.");
                    Console.ReadLine();
                    Login(bugService, userService);
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
                "Menu" +
                "\n==============================" +
                "\n1. Report Bug\n" +
                "2. View Bugs\n" +
                "3. Update Bug\n" +
                "4. Delete Bug\n" +
                "5. Exit"
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
                UpdateBug(bugSer, userSer, user);
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
                ViewAllUsers(bugSer, userSer, user);
            }
            if (MenuChoice == "8" && user.UserRole == Role.Admin)
            {
                Console.Clear();
                AssignDeveloperMenu(bugSer, userSer, user);
            }
            if (MenuChoice == "9" && user.UserRole == Role.Admin)
            {
                Console.Clear();
                UserPromotionMenu(bugSer, userSer, user);
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
        /// Run a menu of diferent ways to view the bugs.
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
                PrintBugs(bugSer, user);

                Console.WriteLine("\nEnter bug ID to get bug details");

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

                PrintBugDetails(bugSer, userSer, user, int.Parse(bugId));
                CommentMenu(bugSer, userSer, user, int.Parse(bugId));
            }

            Console.Clear();
            ShowMenu(bugSer, userSer, user);
        }
        /// <summary>
        /// Runs through different menus to update bugs based on the users role.
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="userSer"></param>
        /// <param name="user"></param>
        private static void UpdateBug(BugService bugSer, UserService userSer,  User user)
        {
            if (user.UserRole == Role.Admin || user.UserRole == Role.Developer)
            {
                PrintBugs(bugSer, user);

                Console.WriteLine("What is the ID of the bug you are trying to change");

                int bugId = int.Parse(Console.ReadLine());

                Console.Clear();
                Console.WriteLine("What would you like to do to bug:  " + bugId + "?");
                Console.WriteLine(
                    "\n" +
                    "1. Close Bug\n" +
                    "2. Advance Bug Status\n"
                );

                ReportedBug bug = bugSer.GetBugById(bugId);
                int input = int.Parse(Console.ReadLine());

                if (input == 1)
                {
                    bug.Status = Status.Closed;
                }
                if (input == 2)
                {
                    Status oldStat = bug.Status;
                    string sOldStat = oldStat.ToString();
                    string sCurrStat = (bug.Status).ToString();
                    bugSer.NextStatus(bugId);
                    Console.WriteLine("Bug Status changed from " + sOldStat + " to " + sCurrStat);
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Your Bugs: \n");

                foreach (var bug in bugSer.GetAllBugs().Where(b => b.AuthorId == user.Id))
                {
                    Console.WriteLine(
                        bug.Id + ": " + bug.Title + "\n" +
                        bug.Description + "\n" +
                        "Uploaded by - " + user.Name + "\n"
                    );
                }

                Console.WriteLine("Which bug would you like to edit");
                int bugId = int.Parse(Console.ReadLine());

                ReportedBug bugToEdit = bugSer.GetBugById(bugId);

                if (bugToEdit.AuthorId == user.Id)
                {
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
                ViewBugs(bugSer, userSer, user);
            }
            ShowMenu(bugSer, userSer, user);
        }
        /// <summary>
        /// Allows general users to delete their own bugs, and allows users with higher privlages to delete any bug.
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="userSer"></param>
        /// <param name="user"></param>
        private static void DeleteBug(BugService bugSer, UserService userSer, User user)
        {
            if (user.UserRole == Role.Admin || user.UserRole == Role.Developer)
            {
                foreach (var bug in bugSer.GetAllBugs())
                {
                    Console.WriteLine(
                        bug.Id + ": " + bug.Title + "\n" +
                        bug.Description + "\n" +
                        "Uploaded by - " + user.Name + "\n"
                    );
                }

                Console.WriteLine("Which bug would you like to delete");
                int bugId = int.Parse(Console.ReadLine());

                ReportedBug bugToDelete = bugSer.GetBugById(bugId);
                if (bugToDelete != null)
                {
                    Console.Clear();
                    bugSer.DeleteBug(bugId);
                    Console.WriteLine("Bug deleted successfully");
                    Console.ReadLine();
                    Console.Clear();
                    DeleteBug(bugSer, userSer, user);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("This Bug does not exist");
                    Console.ReadLine();
                    Console.Clear();
                    DeleteBug(bugSer, userSer, user);
                }
            }
            if (user.UserRole == Role.GeneralUser)
            {
                Console.WriteLine("Your Bugs: \n");
                foreach (var bug in bugSer.GetAllBugs().Where(Bug => Bug.AuthorId == user.Id))
                {
                    Console.WriteLine(
                        "\n-------------------------\n" +
                        bug.Id + ": " + bug.Title + ": \n" + 
                        bug.Description + 
                        "\n-------------------------\n"
                    );
                    Console.WriteLine("Which bug would you like to delete?");

                    int bugId = int.Parse(Console.ReadLine());

                    ReportedBug bugToDelete = bugSer.GetBugById(bugId);
                    if (bugToDelete != null)
                    {
                        bugSer.DeleteBug(bugToDelete.Id);
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
            ShowMenu(bugSer, userSer, user);
        }
        //----------------- Developer Menus -----------------
        /// <summary>
        /// Allows higher privlage users to assign severity to bugs.
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="userSer"></param>
        /// <param name="user"></param>
        public static void AssignSeverity(BugService bugSer, UserService userSer, User user)
        {
            Console.Clear();
            PrintBugs(bugSer, user);

            Console.WriteLine("\nWhich bug do you want to assign a severity level to?");

            int input;
            bool isValid = int.TryParse((Console.ReadLine()), out input);

            if (isValid)
            {
                Console.Clear();
                Console.WriteLine("What Severity do you want to assign to bug " + input + "?" +
                    "\n\n1. Low" +
                    "\n2. Medium" +
                    "\n3. High" +
                    "\n4. Critical\n"
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
        /// <summary>
        /// Allows admins to see all users.
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="userSer"></param>
        /// <param name="user"></param>
        public static void ViewAllUsers(BugService bugSer, UserService userSer, User user)
        {
            PrintAllUsers(userSer);

            Console.WriteLine(
                "\n1. Assign Developer To Bug" +
                "\n2. Promote User To A New Role" +
                "\n3. Back"
            );

            int input;
            bool isValid = int.TryParse((Console.ReadLine()), out input);
            if (isValid)
            {
                if (input == 1)
                {
                    AssignDeveloperMenu(bugSer, userSer, user);
                }
                if (input == 2)
                {
                    UserPromotionMenu(bugSer, userSer, user);
                }
            }

            ShowMenu(bugSer, userSer, user);
        }
        /// <summary>
        /// Allows admins to assign developers to fix a bug.
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="userSer"></param>
        /// <param name="user"></param>
        public static void AssignDeveloperMenu(BugService bugSer, UserService userSer, User user)
        {
            int bugId;
            int userId;

            PrintBugs(bugSer, user);
            Console.WriteLine("\n0. Back\n");
            Console.WriteLine("\nWhich bug would you like to assign a developer to?");

            bool bugValid = int.TryParse((Console.ReadLine()), out bugId);

            if (bugId == 0)
            {
                ShowMenu(bugSer, userSer, user);
            }

            if (!bugValid)
            {
                Console.WriteLine("Invalid bug ID. Please try again.");
                Console.ReadLine();
                AssignDeveloperMenu(bugSer, userSer, user);
            }

            PrintAllUsers(userSer);
            Console.WriteLine("\n0. Back\n");
            Console.WriteLine("\nWhich user would you like to assign to a bug?");

            bool userValid = int.TryParse((Console.ReadLine()), out userId);

            if (userId == 0)
            {
                ShowMenu(bugSer, userSer, user);
            }

            if (userValid && userSer.GetUserById(userId).UserRole == Role.Developer)
            {
                bugSer.AssignDevToBug(bugId, userId);
            }
            else
            {
                Console.WriteLine("Invalid user ID. Please try again.");
                Console.ReadLine();
                AssignDeveloperMenu(bugSer, userSer, user);
            }

                ShowMenu(bugSer, userSer, user);
        }
        /// <summary>
        /// Allows admins change a users role privlages.
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="userSer"></param>
        /// <param name="user"></param>
        public static void UserPromotionMenu(BugService bugSer, UserService userSer, User user)
        {
            PrintAllUsers(userSer);
            Console.WriteLine("\nWhich user would you like to promote?");

            int input;
            bool isValid = int.TryParse((Console.ReadLine()), out input);

            if (isValid)
            {
                User userToPromote = userSer.GetUserById(input);
                if (userToPromote != null)
                {
                    Console.Clear();
                    Console.WriteLine("What role would you like to assign to " + userToPromote.Name + "?" +
                        "\n\n1. General User" +
                        "\n2. Developer" +
                        "\n3. Admin\n"
                    );
                    int roleInput = int.Parse(Console.ReadLine());
                    if (roleInput > 0 && roleInput < 4)
                    {
                        userSer.PromoteUser(userToPromote.Id, roleInput);
                    }
                    else
                    {
                        Console.WriteLine("Invalid role selection.");
                        Console.ReadLine();
                        UserPromotionMenu(bugSer, userSer, user);
                    }
                    Console.WriteLine(userToPromote.Name + " has been promoted to " + userToPromote.UserRole);
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
            }

            ShowMenu(bugSer, userSer, user);
        }

        //----------------- Repeat Menus -----------------
        /// <summary>
        /// Print all bugs found in the _bugs List.
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="user"></param>
        private static void PrintBugs(BugService bugSer, User user)
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
                        "Status: " + bug.Status + 
                        "\nAssigned to: " + bug.AssignedTo + "\n"
                    );
                }
            }
        }
        /// <summary>
        /// Prints a singular bugs details to view.
        /// </summary>
        /// <param name="bugser"></param>
        /// <param name="userSer"></param>
        /// <param name="user"></param>
        /// <param name="bugId"></param>
        private static void PrintBugDetails(BugService bugser, UserService userSer, User user, int bugId)
        {
            Console.Clear();

            ReportedBug bug = bugser.GetBugById(bugId);
            Console.WriteLine(
                    bug.Id + ": " + bug.Title + "\n" +
                    bug.Description + "\n" +
                    "Uploaded by - " + bug.Author + "\n"
            );
            if (user.UserRole == Role.Admin || user.UserRole == Role.Developer)
            {
                Console.WriteLine("Assigned to: " + bug.AssignedTo);
                Console.WriteLine("Severity Level: " + bug.Severity);
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
        /// <summary>
        /// Makes an appended comment menu to put under specific bugs.
        /// </summary>
        /// <param name="bugSer"></param>
        /// <param name="userSer"></param>
        /// <param name="user"></param>
        /// <param name="bugId"></param>
        private static void CommentMenu(BugService bugSer, UserService userSer, User user, int bugId)
        {
            Console.WriteLine("\n\n1. Add Comment");
            Console.WriteLine("2. Back");

            if (user.UserRole == Role.Admin || user.UserRole == Role.Developer)
            {
                Console.WriteLine("\n3. Close Bug");
            }

            string input = Console.ReadLine();

            if (input == "1" || (input == "3" && (user.UserRole == Role.Admin || user.UserRole == Role.Developer)))
            {
                if (input == "1")
                {
                    Console.WriteLine("\nPlease Write Comment");
                    string text = Console.ReadLine();
                    bugSer.AddCommentToBug(bugId, user.Id, text);
                    Console.Clear();
                    PrintBugDetails(bugSer, userSer, user, bugId);
                }
                if (input == "3")
                {
                    bugSer.CloseBug(bugId);
                    Console.WriteLine("Bug status updated to " + bugSer.GetBugById(bugId).Status);
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            else
            {
                Console.Clear();
                ViewBugs(bugSer, userSer, user);
            }
        }
        /// <summary>
        /// Print all users for admins to view and work with.
        /// </summary>
        /// <param name="userSer"></param>
        private static void PrintAllUsers(UserService userSer)
        {
            Console.Clear();
            Console.WriteLine("Bugs:\n================================");

            foreach (var user in userSer.GetAllUsers())
            {
                Console.WriteLine(
                    user.Id + ": " + user.Name + "\n" +
                    "Role: " + user.UserRole + "\n"
                );
            }
        }
    }

}
