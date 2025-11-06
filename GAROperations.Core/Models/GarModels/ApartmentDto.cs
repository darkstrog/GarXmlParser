using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;
public class ApartmentDto
{
    public required long ID {  get; set; }

    public required long OBJECTID { get; set; }

    public required string OBJECTGUID { get; set; }

    public required long CHANGEID { get; set; }

    public required string NUMBER { get; set; }

    public required int APARTTYPE { get; set; }

    public required long OPERTYPEID { get; set; }

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
}
