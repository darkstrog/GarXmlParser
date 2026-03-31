namespace GAROperations.Core.Models.GarModels;

public class NormativeDocKindDto
{
    public required int ID {  get; set; }

    public required string NAME { get; set; }

    public string? OriginalXMLString { get; set; }

    public string? XmlFilePath { get; set; }
}