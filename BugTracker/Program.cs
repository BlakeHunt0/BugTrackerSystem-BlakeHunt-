using System.Reflection.Metadata;

namespace BugTracker
{
    public class Program
    {
        static void Main(string[] args)
        {
            //TODO: make a better list initializer for bugs, and later users.
            //Stanky way to init the bugs list
            BugService bugService = new BugService();

            Console.WriteLine("Bugs:\n================================");

            foreach (var bug in bugService.GetAllBugs())
            {
                Console.WriteLine(bug.Id + ": " + bug.Title + ":    " + bug.Description);
            }
            Console.Clear();

            //Login();
            ShowMenu();
        }

        //Menus
        public static int Login()
        {
            //this is sort of a trick, this might count as a magic number. Think about changing later.
            int userId = 0;

            while (userId == 0)
            {
                Console.Clear();
                Console.WriteLine("Please enter your username: ");
                string inputname = Console.ReadLine();
                Console.WriteLine("Please enter your password: ");
                string inputpassword = Console.ReadLine();

                User curUser = User.users.FirstOrDefault(u => u.Name == inputname && u.Password == inputpassword);

                if (curUser != null)
                {
                    userId = curUser.Id;
                    Console.WriteLine($"Welcome {curUser.Name}!");
                    ShowMenu();
                }
                else
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                    break;
                }
            }

            return userId;
        }
        private static void ShowMenu()
        {
            Console.WriteLine("1. Report Bug");
            Console.WriteLine("2. View Bugs");
            Console.WriteLine("3. Update Bug");
            Console.WriteLine("4. Delete Bug");
            Console.WriteLine("5. Add Comment");
            Console.WriteLine("6. Exit");

            string MenuChoice = Console.ReadLine();

            if (MenuChoice == "1")
            {
                Console.Clear();
                ReportBug();
            }
            if (MenuChoice == "2")
            {
                Console.Clear();
                ViewBugs();
            }

            if (MenuChoice == "6")
            {
               Environment.Exit(0);
            }
        }

        //Menu Choices
        private static void ReportBug()
        {
            //look into a way of getting this line to work across program.cs
            BugService bugService = new BugService();

            Console.Clear();

            Console.WriteLine("Enter Title");
            string Title = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Enter Description");
            string Description = Console.ReadLine();

            //TODO: add the uploading user to the new bug.

            bugService.AddBug(Title, Description);

            Console.Clear();
            ShowMenu();
        }
        private static void ViewBugs()
        {
            BugService bugService = new BugService();

            Console.WriteLine("Bugs:\n================================");

            foreach (var bug in bugService.GetAllBugs())
            {
                Console.WriteLine(bug.Id + ": " + bug.Title + ":    " + bug.Description);
            }

            Console.ReadLine();

            //it has occured to me that these menus are layering atop eachother, which might cause problems.
            Console.Clear();
            ShowMenu();
        }
        //this should only be for admins or developers
        private static void UpdateBug(int userRoleInt, int bugId)
        {
            //make sure that the user is an admin or developer
            if (userRoleInt == 2 || userRoleInt == 3)
            {
                Console.WriteLine("What would you like to do to bug:" + bugId + "?");
                Console.WriteLine("1. Close Bug");
            }
        }
        //everyone can get this menu, but general users can only delete their own bugs
        public static void DeleteBug()
        {

        }
        public static void AddComment()
        {

        }
    }

}
