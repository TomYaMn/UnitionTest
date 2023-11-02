using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {

        // Setup configuration
        var configuration = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            .SetBasePath(GetDirectoryOfCurrentFile())
            .AddJsonFile("appsettings.json")
            .Build();

        // Setup Dependency Injection
        var serviceProvider = new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
            .BuildServiceProvider();

        // Get an instance of the ApplicationDbContext
        var dbContext = serviceProvider.GetService<ApplicationDbContext>();

        // Ensure the database is created and migrated
        dbContext.Database.Migrate();

        // Create a new bug
        var bug = new Bug
        {
            Summary = "Test Bug",
            Description = "This is a test bug",
            Resolved = false
        };

        // Use the BugController to create the bug
        var bugController = new BugController(dbContext);
        bugController.CreateBug(bug).GetAwaiter().GetResult();

        // Display the bugs
        var bugs = bugController.GetBugs().Result;
        Console.WriteLine("Bugs in the database:");
        foreach (var b in bugs.Value)
        {
            Console.WriteLine($"Id: {b.Id}, Summary: {b.Summary}, Resolved: {b.Resolved}");
        }

        // Wait for user input before closing the console
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static string GetDirectoryOfCurrentFile([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
    {
        return Path.GetDirectoryName(sourceFilePath);
    }

}