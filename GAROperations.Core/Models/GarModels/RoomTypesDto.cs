using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;

public class RoomTypeDto
{
    public required int ID { get; set; }

    public required string NAME { get; set; }

    public required string SHORTNAME { get; set; }

    public required string DESC { get; set; }

    [DataType(DataType.Date)]
    public required DateTime UPDATEDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime STARTDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime ENDDATE { get; set; }

    public required bool ISACTIVE { get; set; }
}