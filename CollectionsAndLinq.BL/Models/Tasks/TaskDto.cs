namespace CollectionsAndLinq.BL.Models.Tasks;

public record TaskDto(
    int Id,
    string Name,
    string Description,
    string State,
    DateTime CreatedAt,
    DateTime? FinishedAt)
{

}
