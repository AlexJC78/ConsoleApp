namespace ConsoleApplication
{
    using Consoleapplication.Data;
    using Consoleapplication.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class Startup
    {
        static void Main(string[] args)
        {
            // Window initialization
            Console.WindowHeight = 17;
            Console.WindowWidth = 50;
            Console.BufferHeight = 17;
            Console.BufferWidth = 50;
            Console.CursorVisible = false;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            // DB init
            var context = new SoftUniContext();

            ListAll(context);
            
        }

        static void ListAll(SoftUniContext context)
        {
            var projectsPaginator = new Paginator(
                context.Projects
                .Select(p => new
                {
                    p.ProjectID,
                    p.Name
                }).ToList()
                .Select(p => $"{p.ProjectID,4}| {p.Name}").ToList(), 2, 0, 14, true);

            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.Clear();
                Console.WriteLine($" ID | Project Name (Page {projectsPaginator.CurrentPage + 1} of {projectsPaginator.MaxPages})");
                Console.WriteLine("----+---------------------------");

                projectsPaginator.Print();
                var key = Console.ReadKey(true);

                if (!KeyboardController.PageController(key, projectsPaginator)) return;

        
    }
}

        static void ShowDetails(Project project)
        {
            //----------------------
            Console.Clear();
            Console.WriteLine($"ID: {project.ProjectID,4}| {project.Name}");
            Utility.PrintHLine();
            Console.WriteLine(project.Description);
            Utility.PrintHLine();
            Console.WriteLine($"{project.StartDate,-24}| {project.EndDate}");
            Utility.PrintHLine();
            Console.WriteLine($"Page");
            Console.WriteLine("------------------------------");


            int pageSize = 16 - Console.CursorTop;

                var employees = project.Employees.ToList();
                int page = 0;
                int maxPages = (int)Math.Ceiling(employees.Count / (double)pageSize);
                int pointer = 1;

                

                while (true)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Clear();
                    Console.WriteLine($"ID: {project.ProjectID,4}| {project.Name}");
                    Utility.PrintHLine();
                    Console.WriteLine(project.Description);
                    Utility.PrintHLine();
                    Console.WriteLine($"{project.StartDate,-24}| {project.EndDate}");
                    Utility.PrintHLine();
                    Console.WriteLine($"(Page {page + 1} of {maxPages})");
                    Console.WriteLine("------------------------------");

                    int current = 1;
                    foreach (var emp in employees.Skip(pageSize * page).Take(pageSize))
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;

                        if (current == pointer)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        Console.WriteLine($"{emp.FirstName} {emp.LastName}");
                        current++;
                    }

                    var key = Console.ReadKey(true);

                    switch (key.Key.ToString())
                    {
                        /*
                        case "Enter":
                            var currentProject = employees.Skip(pageSize * page + pointer - 1).First();
                            ShowDetails(currentProject);
                            break;
                            */
                        case "UpArrow":
                            if (pointer > 1)
                            {
                                pointer--;
                            }
                            else if (page > 0)
                            {
                                page--;
                                pointer = pageSize;
                            }
                            break;
                        case "DownArrow":
                            if (pointer < pageSize)
                            {
                                pointer++;
                            }
                            else if (page + 1 < maxPages)
                            {
                                page++;
                                pointer = 1;
                            }
                            break;
                        case "Escape":
                            return;
                    }
                }
            }
        //---------------------
    }
}
