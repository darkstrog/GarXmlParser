using System.ComponentModel.DataAnnotations;

namespace GAROperations.Core.Models.GarModels;
//TODO: Сделать отдельные сервисы на сущности (Entity)Param
public class ParamDto
{
    public required long ID {  get; set; }

    public required long OBJECTID { get; set; }

    public long? CHANGEID { get; set; }

    public required long CHANGEIDEND { get; set; }

    public required int TYPEID { get; set; }

    public required string VALUE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime UPDATEDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime STARTDATE { get; set; }

    [DataType(DataType.Date)]
    public required DateTime ENDDATE { get; set; }
}
