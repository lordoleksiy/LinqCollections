namespace CollectionsAndLinq.BL.Models.Projects;

public record FilterModel(
    string Name = null,
    string Description = null,
    string AutorFirstName = null,
    string AutorLastName = null,
    string TeamName = null)
{

}
