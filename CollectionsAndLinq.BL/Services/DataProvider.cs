using System;
using System.Collections.Generic;
using System.Linq;
using CollectionsAndLinq.BL.Entities;
using CollectionsAndLinq.BL.Interfaces;

namespace CollectionsAndLinq.BL.Services;

public class DataProvider : IDataProvider
{
    private readonly Client _client;
    
    public DataProvider()
    {
        _client = new Client();
    }
    public Task<List<Project>> GetProjectsAsync()
    {
        return _client.Get<List<Project>>("Projects");
    }

    public Task<List<Entities.Task>> GetTasksAsync()
    {
        return _client.Get<List<Entities.Task>>("Tasks");
    }

    public Task<List<Team>> GetTeamsAsync()
    {
        return _client.Get<List<Team>>("Teams");
    }

    public Task<List<User>> GetUsersAsync()
    {
        return _client.Get<List<User>>("Users");
    }

    public Task<Project> GetProjectAsync(int id)
    {
        return _client.Get<Project>($"Projects/{id}");
    }

    public Task<Entities.Task> GetTaskAsync(int id)
    {
        return _client.Get<Entities.Task>($"Tasks/{id}");
    }

    public Task<Team> GetTeamAsync(int id)
    {
        return _client.Get<Team>($"Teams/{id}");
    }

    public Task<User> GetUserAsync(int id)
    {
        return _client.Get<User>($"Users/{id}");
    }
}
