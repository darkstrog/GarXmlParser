namespace GAROperations.Core.Models.GarModels;
public partial class AddressObjectDivisionItemDto
{
    public long ID { get; set; }

    public long PARENTID { get; set; }

    public long CHILDID { get; set; }

    public long CHANGEID { get; set; }
    public string? OriginalXMLString { get; set; }

    public string? XmlFilePath { get; set; }
}
