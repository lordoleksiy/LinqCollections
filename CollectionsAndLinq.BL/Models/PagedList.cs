namespace CollectionsAndLinq.BL.Models;

public record PagedList<T>(
    List<T> Items,
    int TotalCount)
{

}
