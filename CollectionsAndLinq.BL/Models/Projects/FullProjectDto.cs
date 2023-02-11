using CollectionsAndLinq.BL.Models.Tasks;
using CollectionsAndLinq.BL.Models.Teams;
using CollectionsAndLinq.BL.Models.Users;

namespace CollectionsAndLinq.BL.Models.Projects;

public record FullProjectDto(
    int Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    DateTime Deadline,
    List<TaskWithPerfomerDto> Tasks,
    UserDto Author,
    TeamDto Team)
{

}
