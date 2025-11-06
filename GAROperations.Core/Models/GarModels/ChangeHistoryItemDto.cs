using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;
public class ChangeHistoryItemDto
{
    public required long CHANGEID { get; set; }

    public required long OBJECTID { get; set; }

    public required string ADROBJECTID { get; set; }

    public required int OPERTYPEID { get; set; }
    
    public long? NDOCID { get; set; }

    [DataType(DataType.Date)]
    public required DateTime CHANGEDATE { get; set; }
}