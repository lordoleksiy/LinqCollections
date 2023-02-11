using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionsAndLinq.BL.Entities;
using CollectionsAndLinq.BL.Interfaces;
using CollectionsAndLinq.BL.Services;

namespace CollectionsAndLinq.PL;

public class ProjectController
{
    private readonly IDataProvider _provider;
    private readonly IDataProcessingService _processingService;
    private bool isOpen = true;
    public ProjectController() {
        _provider= new DataProvider();
        _processingService = new DataProcessingService(_provider);
    }
    public void start()
    {
        Hello();

        while (isOpen)
        {
            string command = GetCommand();
            try
            {
                switch (command)
                {
                    case "/task1":
                        GetTask1();
                        break;
                    case "/task2":
                        GetTask2();
                        break;
                    case "/task3":
                        GetTask3();
                        break;
                    case "/task4":
                        GetTask4();
                        break;
                    case "/task5":
                        GetTask5();
                        break;
                    case "/task6":
                        GetTask6();
                        break;
                    case "/task7":
                        GetTask7();
                        break;
                    case "/task8":
                        break;
                    case "/help":
                        Info();
                        break;
                    case "/exit":
                        isOpen = false;
                        break;
                    default:
                        Console.WriteLine("No such command");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine(ex.Message.ToString());
                Console.WriteLine("----------------------------------------");
            }
        }
    }

    public void GetTask1()
    {
        Console.Write("Enter userId: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out int temp))
        {
            Console.WriteLine("Wrong input...");
            return;
        }

        var res = _processingService.GetTasksCountInProjectsByUserIdAsync(temp);
        res.Wait();
        foreach (var r in res.Result)
        {
            Console.WriteLine($"key: {r.Key}; value: {r.Value}");
        }
    }

    public void GetTask2()
    {
        Console.Write("Enter userId: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out int temp))
        {
            Console.WriteLine("Wrong input...");
            return;
        }

        var res = _processingService.GetCapitalTasksByUserIdAsync(temp);
        foreach (var task in res.Result)
        {
            Console.WriteLine($"{task.Id} {task.Name} {task.Description} {task.State.ToString()} {task.CreatedAt} {task.FinishedAt}");
        }
    }

    public void GetTask3()
    {
        Console.Write("Enter teamsize: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out int temp))
        {
            Console.WriteLine("Wrong input...");
            return;
        }
        var res = _processingService.GetProjectsByTeamSizeAsync(temp);
        foreach (var cortage in res.Result)
        {
            Console.WriteLine($"{cortage.Id} {cortage.Name}");
        }
    }
    public void GetTask4()
    {
        Console.Write("Enter year: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out int temp))
        {
            Console.WriteLine("Wrong input...");
            return;
        }
        var res = _processingService.GetSortedTeamByMembersWithYearAsync(temp);
        foreach (var team in res.Result)
        {
            Console.WriteLine("====================TEAM===========:");
            Console.WriteLine($"{team.Id}: {team.Name}");
            Console.WriteLine("-----Members:------");
            foreach (var member in team.Members)
            {
                Console.WriteLine($"{member.Id}|{member.Email}|{member.FirstName}|{member.LastName}|{member.BirthDay}");
            }
        }
    }
    public void GetTask5()
    {
        var res = _processingService.GetSortedUsersWithSortedTasksAsync();
        foreach (var user in res.Result)
        {
            Console.WriteLine("====================USER===========:");
            Console.WriteLine($"{user.Id}: {user.Email}|{user.LastName}|{user.FirstName}|{user.RegisteredAt}|{user.BirthDay}");
            Console.WriteLine("-----Tasks:------");
            foreach (var task in user.Tasks)
            {
                Console.WriteLine($"{task.Id}|{task.Name}|{task.CreatedAt}|{task.State}|{task.Description}");
            }
        }
    }
    public void GetTask6()
    {

        Console.Write("Enter userId: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out int temp))
        {
            Console.WriteLine("Wrong input...");
            return;
        }
        var res = _processingService.GetUserInfoAsync(temp).Result;
        var user = res.User;
        var lastProject = res.LastProject;
        var task = res.LongestTask;
        Console.WriteLine($"{user.Id}: {user.Email}|{user.LastName}|{user.FirstName}|{user.RegisteredAt}|{user.BirthDay}");
        Console.WriteLine($"Count of cancelled or not finished tasks: {res.NotFinishedOrCanceledTasksCount}");
        Console.WriteLine("-----Last Project--------");
        Console.WriteLine($"{lastProject.Id}|{lastProject.Name}|{lastProject.Deadline}|{lastProject.CreatedAt}|{lastProject.Deadline}");
        Console.WriteLine("-----Longest Task--------");
        Console.WriteLine($"{task.Id}|{task.Name}|{task.CreatedAt}|{task.State}|{task.Description}");
    }

    public void GetTask7()
    {
        var res = _processingService.GetProjectsInfoAsync().Result;
        foreach (var project in res)
        {
            var pr = project.Project;
            var task1 = project.ShortestTaskByName;
            var task2 = project.LongestTaskByDescription;
            Console.WriteLine("-----Project--------");
            Console.WriteLine($"{pr.Id}|{pr.Name}|{pr.Deadline}|{pr.CreatedAt}|{pr.Deadline}");
            Console.WriteLine($"Count: {project.TeamMembersCount}");
            Console.WriteLine("-----Longest Task--------");
            if (task1 is not null)
                Console.WriteLine($"{task1.Id}|{task1.Name}|{task1.CreatedAt}|{task1.State}|{task1.Description}");
            Console.WriteLine("-----Shortest Task--------");
            if (task2 is not null)
                Console.WriteLine($"{task2.Id} | {task2.Name} | {task2.CreatedAt} | {task2.State} | {task2.Description}");
        }
    }
    private static void Info()
    {
        Console.WriteLine("Choose command from the list:\n\n" +
            "\t/task1 - GetTasksCountInProjectsByUserIdAsync\n" +
            "\t/task2 - GetCapitalTasksByUserIdAsync\n" +
            "\t/task3 - GetProjectsByTeamSizeAsync\n" +
            "\t/task4 - GetSortedTeamByMembersWithYearAsync\n" +
            "\t/task5 - GetSortedUsersWithSortedTasksAsync\n" +
            "\t/task6 - GetUserInfoAsync\n" +
            "\t/task7 - GetProjectsInfoAsync\n" +
            "\t/task8 - GetSortedFilteredPageOfProjectsAsync\n");
    }
    public void Hello()
    {
        Console.WriteLine("Welcome to our programm. Enter commands...");
    }

    private static string GetCommand()
    {
        Console.Write("Enter here: ");
        var command = Console.ReadLine();
        return command!;
    }
}
