using CollectionsAndLinq.BL.Models.Projects;
using CollectionsAndLinq.BL.Models.Tasks;

namespace CollectionsAndLinq.BL.Models.Users;

public record UserInfoDto(
    UserDto User,
    ProjectDto LastProject,
    int LastProjectTasksCount,
    int NotFinishedOrCanceledTasksCount,
    TaskDto LongestTask)
{

}
