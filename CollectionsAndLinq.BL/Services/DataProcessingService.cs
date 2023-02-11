using System.Linq;
using System.Reflection.Metadata.Ecma335;
using CollectionsAndLinq.BL.BLData;
using CollectionsAndLinq.BL.Entities;
using CollectionsAndLinq.BL.Interfaces;
using CollectionsAndLinq.BL.Models;
using CollectionsAndLinq.BL.Models.Projects;
using CollectionsAndLinq.BL.Models.Tasks;
using CollectionsAndLinq.BL.Models.Teams;
using CollectionsAndLinq.BL.Models.Users;
using Task = System.Threading.Tasks.Task;

namespace CollectionsAndLinq.BL.Services;

// Add implementations to the methods and constructor. You can also add new members to the class.
public class DataProcessingService : IDataProcessingService
{
    private readonly DataProvider _dataProvider;
    public DataProcessingService(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider as DataProvider;
    }

    public Task<Dictionary<string, int>> GetTasksCountInProjectsByUserIdAsync(int userId)
    {
        var tasks = _dataProvider.GetTasksAsync().Result;
        var projects = _dataProvider.GetProjectsAsync().Result;
        var user = _dataProvider.GetUserAsync(userId).Result;
        var res = from project in projects
                  where project.TeamId.Equals(user.TeamId)
                  let count = tasks.Where(a => a.ProjectId.Equals(project.Id) && a.PerformerId.Equals(userId)).Count()
                  select new { key = $"{project.Id}: {project.Name}", value = count };

        return Task.FromResult(res.ToDictionary(a => a.key, a => a.value));
    }

    public Task<List<TaskDto>> GetCapitalTasksByUserIdAsync(int userId)
    {
        var res = from tasks in _dataProvider.GetTasksAsync().Result
                  where tasks.PerformerId == userId && char.IsUpper(tasks.Name[0])
                  select new TaskDto(
                      tasks.Id,
                      tasks.Name,
                      tasks.Description,
                      Data.TaskStateMapper.GetValueOrDefault((int)tasks.State),
                      tasks.CreatedAt,
                      tasks.FinishedAt);

        return Task.FromResult(res.ToList());
    }

    public Task<List<(int Id, string Name)>> GetProjectsByTeamSizeAsync(int teamSize)
    {
        var projects = _dataProvider.GetProjectsAsync().Result;
        var users = _dataProvider.GetUsersAsync().Result;

        var res = from project in projects
                  where users.Where(a => a.TeamId.Equals(project.TeamId)).Count() > teamSize
                  select (project.Id, project.Name);

        return Task.FromResult(res.ToList());
    }

    public Task<List<TeamWithMembersDto>> GetSortedTeamByMembersWithYearAsync(int year)
    {
        var teams = _dataProvider.GetTeamsAsync().Result;
        var members = _dataProvider.GetUsersAsync().Result;
        var res = from team in teams
                  orderby team.Name
                  let teamMembers = members.Where(a => a.TeamId.Equals(team.Id))
                  where teamMembers.Any() && teamMembers.All(a => a.BirthDay.Year < year)
                  select new TeamWithMembersDto(team.Id, team.Name, teamMembers.OrderByDescending(a => a.BirthDay)
                  .Select(a => new UserDto(a.Id, a.FirstName, a.LastName, a.Email, a.RegisteredAt, a.BirthDay)).ToList()
                  );

        return Task.FromResult(res.ToList());
    }

    public Task<List<UserWithTasksDto>> GetSortedUsersWithSortedTasksAsync()
    {
        var users = _dataProvider.GetUsersAsync().Result;
        var tasks = _dataProvider.GetTasksAsync().Result;
        var res = from user in users
                  orderby user.FirstName
                  let userTasks = tasks.Where(a => a.PerformerId.Equals(user.Id)).OrderByDescending(a => a.Name.Length)
                  select new UserWithTasksDto(user.Id, user.FirstName, user.LastName, user.Email, user.RegisteredAt, user.BirthDay, userTasks
                  .Select(a => new TaskDto(a.Id, a.Name, a.Description, Data.TaskStateMapper.GetValueOrDefault((int)a.State), a.CreatedAt, a.FinishedAt)).ToList());

        return Task.FromResult(res.ToList());

    }

    public Task<UserInfoDto> GetUserInfoAsync(int userId)
    {
        var user = _dataProvider.GetUserAsync(userId).Result;
        var projects = _dataProvider.GetProjectsAsync().Result;
        var tasks = _dataProvider.GetTasksAsync().Result;

        var userDto = new UserDto(user.Id, user.FirstName, user.LastName, user.Email, user.RegisteredAt, user.BirthDay);
        var res = (from project in projects
                    where project.TeamId == user.TeamId
                    orderby project.CreatedAt descending
                    let projectDto = new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt, project.Deadline)
                    let projectTasks = from task in tasks
                                       where task.PerformerId == userId && task.ProjectId == project.Id
                                       select task
                    let tasksCount = projectTasks.Count()
                    let cancelTasks = (from task in projectTasks
                                       where task.State.Equals(TaskState.Canceled) || task.State.Equals(TaskState.InProgress) || task.State.Equals(TaskState.ToDo)
                                       select task).Count()
                    let maxTimeTask = projectTasks.Max(a =>
                    {
                        if (a.FinishedAt != null)
                            return (a.FinishedAt.Value - a.CreatedAt).Milliseconds;
                        return (DateTime.Now - a.CreatedAt).Milliseconds;
                    })
                    let longestTask = projectTasks.FirstOrDefault(a =>
                    {
                        if (a.FinishedAt != null)
                            return (a.FinishedAt.Value - a.CreatedAt).Milliseconds.Equals(maxTimeTask);
                        return (DateTime.Now - a.CreatedAt).Milliseconds.Equals(maxTimeTask);
                    })
                    let longestTaskDto = new TaskDto(longestTask.Id, longestTask.Name, longestTask.Description, Data.TaskStateMapper.GetValueOrDefault((int)longestTask.State), longestTask.CreatedAt, longestTask.FinishedAt)
                    select new UserInfoDto(userDto, projectDto, tasksCount, cancelTasks, longestTaskDto)).FirstOrDefault();

        return Task.FromResult(res);
    }

    public Task<List<ProjectInfoDto>> GetProjectsInfoAsync()
    {
        var projects = _dataProvider.GetProjectsAsync().Result;
        var tasks = _dataProvider.GetTasksAsync().Result;
        var users = _dataProvider.GetUsersAsync().Result;


        var res = from project in projects
                  let ptasks = from task in tasks
                               where task.ProjectId == project.Id
                               select task
                  let maxDescriptionLength = ptasks.MaxBy(a => a.Description.Length)
                  let shortestName = ptasks.MinBy(a => a.Name.Length)
                  let task1 = (from task in ptasks
                               where task.Description.Length.Equals(maxDescriptionLength)
                               select task).FirstOrDefault()
                  let task2 = (from task in ptasks
                               where task.Name.Length.Equals(shortestName)
                               select task).FirstOrDefault()
                  let count = (from user in users
                               where user.TeamId == project.TeamId
                               where project.Description.Length > 20 && ptasks.Count() > 3 // how to make null if this condition is false? 
                               select user).Count()

                  let projectDto = new ProjectDto(project.Id, project.Name, project.Description, project.CreatedAt, project.Deadline)
                  let task1Dto = task1 is not null? new TaskDto(task1.Id, task1.Name, task1.Description, Data.TaskStateMapper.GetValueOrDefault((int)task1.State), task1.CreatedAt, task1.FinishedAt): null
                  let task2Dto = task2 is not null? new TaskDto(task2.Id, task2.Name, task2.Description, Data.TaskStateMapper.GetValueOrDefault((int)task2.State), task2.CreatedAt, task2.FinishedAt): null
                  select new ProjectInfoDto(projectDto, task1Dto, task2Dto, count);

        return Task.FromResult(res.ToList());
    }

    public Task<PagedList<FullProjectDto>> GetSortedFilteredPageOfProjectsAsync(PageModel pageModel, FilterModel filterModel, SortingModel sortingModel)
    {
        //var res = null;
        //throw new NotImplementedException();
        return null;
    }
}
