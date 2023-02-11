namespace CollectionsAndLinq.BL.Entities;

public record Task(
    int Id,
    int ProjectId,
    int PerformerId,
    string Name,
    string Description,
    TaskState State,
    DateTime CreatedAt,
    DateTime? FinishedAt)
{

}
