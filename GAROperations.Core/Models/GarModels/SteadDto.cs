using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;

public class SteadDto
{
    public required int ID { get; set; }

    public required int OBJECTID { get; set; }

    public required string OBJECTGUID { get; set; }

    public required int CHANGEID { get; set; }

    public required string NUMBER { get; set; }

    public required string OPERTYPEID { get; set; }

    public required int PREVID { get; set; }

    public required int NEXTID { get; set; }

    [DataType(DataType.Date)]
    public required DateTime UPDATEDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime STARTDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime ENDDATE { get; set; }

    public required bool ISACTUAL { get; set; }

    public required bool ISACTIVE { get; set; }
}