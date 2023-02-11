using CollectionsAndLinq.BL.Models;
using CollectionsAndLinq.BL.Models.Projects;
using CollectionsAndLinq.BL.Models.Tasks;
using CollectionsAndLinq.BL.Models.Teams;
using CollectionsAndLinq.BL.Models.Users;

namespace CollectionsAndLinq.BL.Interfaces;

public interface IDataProcessingService
{
    Task<Dictionary<string, int>> GetTasksCountInProjectsByUserIdAsync(int userId);
    Task<List<TaskDto>> GetCapitalTasksByUserIdAsync(int userId);
    Task<List<(int Id, string Name)>> GetProjectsByTeamSizeAsync(int teamSize);
    Task<List<TeamWithMembersDto>> GetSortedTeamByMembersWithYearAsync(int year);
    Task<List<UserWithTasksDto>> GetSortedUsersWithSortedTasksAsync();
    Task<UserInfoDto> GetUserInfoAsync(int userId);
    Task<List<ProjectInfoDto>> GetProjectsInfoAsync();
    Task<PagedList<FullProjectDto>> GetSortedFilteredPageOfProjectsAsync(PageModel pageModel, FilterModel filterModel, SortingModel sortingModel);
}

