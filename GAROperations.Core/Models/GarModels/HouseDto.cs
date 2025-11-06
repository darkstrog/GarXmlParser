using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;

public class HouseDto
{
    public required long ID { get; set; }

    public required long OBJECTID { get; set; }

    public required string OBJECTGUID { get; set; }

    public required long CHANGEID { get; set; }

    public required string HOUSENUM { get; set; }

    public required string ADDNUM1 { get; set; }

    public required string ADDNUM2 { get; set; }

    public required int HOUSETYPE { get; set; }

    public required int ADDTYPE1 { get; set; }

    public required int ADDTYPE2 { get; set; }

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
}