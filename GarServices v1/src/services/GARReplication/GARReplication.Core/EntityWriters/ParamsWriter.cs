using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public abstract class ParamsWriter: IEntityWriter
    {
        public abstract string TypeName {  get; }
        public string InsertQuery => $"""
                                        COPY {TypeName}
                                            (id, objectid, changeid, changeidend, typeid,
                                            value, updatedate, startdate, enddate)
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public abstract string FilePattern { get; }

        public string NodeName => "PARAM";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((long)entity.Attribute(_IDName), NpgsqlDbType.Bigint);
            writer.Write((long)entity.Attribute(_OBJECTIDName), NpgsqlDbType.Bigint);
            writer.Write((long?)entity.Attribute(_CHANGEIDName), NpgsqlDbType.Bigint);
            writer.Write((long)entity.Attribute(_CHANGEIDENDName), NpgsqlDbType.Bigint);
            writer.Write((int)entity.Attribute(_TYPEIDName), NpgsqlDbType.Integer);
            writer.Write((string)entity.Attribute(_VALUEName), NpgsqlDbType.Varchar);
            writer.Write((DateTime)entity.Attribute(_UPDATEDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_STARTDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_ENDDATEName), NpgsqlDbType.Date);

        }

    }

    public class AddressObjectParamsWriter : ParamsWriter
    {
        public override string FilePattern => @"AS_ADDR_OBJ_PARAMS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
        public override string TypeName => "addr_obj_params";
    }
    public class ApartmentsParamsWriter : ParamsWriter
    {
        public override string FilePattern => @"AS_APARTMENTS_PARAMS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
        public override string TypeName => "apartments_params";
    }
    public class CarPlaceParamsWriter : ParamsWriter
    {
        public override string FilePattern => @"AS_CARPLACES_PARAMS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
        public override string TypeName => "carplaces_params";
    }
    public class HousesParamsWriter : ParamsWriter
    {
        public override string FilePattern => @"AS_HOUSES_PARAMS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
        public override string TypeName => "houses_params";
    }
    public class RoomsParamsWriter : ParamsWriter
    {
        public override string FilePattern => @"AS_ROOMS_PARAMS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
        public override string TypeName => "rooms_params";
    }
    public class SteadsParamsWriter : ParamsWriter
    {
        public override string FilePattern => @"AS_STEADS_PARAMS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
        public override string TypeName => "steads_params";
    }
}
