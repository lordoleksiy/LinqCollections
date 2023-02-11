using CollectionsAndLinq.BL.Models.Tasks;

namespace CollectionsAndLinq.BL.Models.Users;

public record UserWithTasksDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime RegisteredAt,
    DateTime BirthDay,
    List<TaskDto> Tasks)
{

}
