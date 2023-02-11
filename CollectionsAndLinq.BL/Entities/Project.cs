namespace CollectionsAndLinq.BL.Entities;

public record Project(
    int Id,
    int AuthorId,
    int TeamId,
    string Name,
    string Description,
    DateTime CreatedAt,
    DateTime Deadline)
{

}
