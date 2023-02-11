using CollectionsAndLinq.BL.Models.Tasks;

namespace CollectionsAndLinq.BL.Models.Projects;

public record ProjectInfoDto(
    ProjectDto Project,
    TaskDto LongestTaskByDescription,
    TaskDto ShortestTaskByName,
    int? TeamMembersCount = null)
{

}
