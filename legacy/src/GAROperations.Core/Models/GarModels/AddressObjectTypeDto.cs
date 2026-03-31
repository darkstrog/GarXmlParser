using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;
public partial class AddressObjectTypeDto
{
    public int ID {  get; set; }

    public int LEVEL { get; set; }

    public string? SHORTNAME { get; set; }

    public string? NAME { get; set; }

    public string? DESC { get; set; }

    [DataType(DataType.Date)]
    public System.DateTime UPDATEDATE { get; set; }

    [DataType(DataType.Date)]
    public System.DateTime STARTDATE { get; set; }

    [DataType(DataType.Date)]
    public System.DateTime ENDDATE { get; set; }
    
    public bool ISACTIVE { get; set; }
    public string? OriginalXMLString { get; set; }

    public string? XmlFilePath { get; set; }
}