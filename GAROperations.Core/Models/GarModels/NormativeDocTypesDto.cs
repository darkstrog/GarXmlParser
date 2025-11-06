using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;
public class NormativeDocTypeDto
{
    public required int ID {  get; set; }

    public required string NAME { get; set; }

    [DataType(DataType.Date)]
    public required DateTime STARTDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime ENDDATE { get; set; }
}