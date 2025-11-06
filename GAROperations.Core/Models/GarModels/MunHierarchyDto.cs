using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;

public class MunHierarchyDto
{
    public required long ID {  get; set; }

    public required long OBJECTID { get; set; }

    public long? PARENTOBJID { get; set; }

    public required long CHANGEID { get; set; }

    public required string OKTMO { get; set; }

    public long? PREVID { get; set; }

    public long? NEXTID { get; set; }

    [DataType(DataType.Date)]
    public required DateTime UPDATEDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime STARTDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime ENDDATE { get; set; }

    public required bool ISACTIVE { get; set; }

    public required string PATH { get; set; }
}
