using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;

public partial class ReestrObjectDto
{
    public required long OBJECTID {  get; set; }

    [DataType(DataType.Date)]
    public required System.DateTime CREATEDATE { get; set; }

    public required long CHANGEID { get; set; }

    public required int LEVELID { get; set; }

    [DataType(DataType.Date)]
    public required DateTime UPDATEDATE { get; set; }

    public required string OBJECTGUID { get; set; }

    public required bool ISACTIVE { get; set; }
}