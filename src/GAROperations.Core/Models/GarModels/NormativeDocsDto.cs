using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;

public class NormativeDocDto
{
    public required long ID {  get; set; }

    public required string NAME { get; set; }

    [DataType(DataType.Date)]
    public required DateTime DATE { get; set; }

    public required string NUMBER { get; set; }
    public required int TYPE { get; set; }

    public required int KIND { get; set; }

    [DataType(DataType.Date)]
    public required DateTime UPDATEDATE { get; set; }

    public required string ORGNAME { get; set; }

    public required string REGNUM { get; set; }

    [DataType(DataType.Date)]
    public required DateTime? REGDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime? ACCDATE { get; set; }

    public required string COMMENT { get; set; }
}
