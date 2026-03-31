using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;

public class AddressObjectDto
{
    public required long ID { get; set; }

    public required long OBJECTID { get; set; }

    public required Guid OBJECTGUID { get; set; }

    public required long CHANGEID { get; set; }

    public required string NAME { get; set; }

    public required string TYPENAME { get; set; }

    public required string LEVEL { get; set; }

    public required int OPERTYPEID { get; set; }

    public long? PREVID { get; set; }

    public long? NEXTID { get; set; }

    [DataType(DataType.Date)]
    public required DateTime UPDATEDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime STARTDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime ENDDATE { get; set; }

    public required bool ISACTUAL { get; set; }

    public required bool ISACTIVE { get; set; }
        
    public string? OriginalXMLString { get; set; }

    public string? XmlFilePath { get; set; }
}