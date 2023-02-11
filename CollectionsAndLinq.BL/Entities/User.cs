namespace CollectionsAndLinq.BL.Entities;

public record User(
    int Id,
    int? TeamId,
    string FirstName,
    string LastName,
    string Email,
    DateTime RegisteredAt,
    DateTime BirthDay)
{

}
