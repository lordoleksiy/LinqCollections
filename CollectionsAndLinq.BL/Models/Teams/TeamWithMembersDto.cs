using CollectionsAndLinq.BL.Models.Users;

namespace CollectionsAndLinq.BL.Models.Teams;

public record TeamWithMembersDto(
    int Id,
    string Name,
    List<UserDto> Members)
{

}
