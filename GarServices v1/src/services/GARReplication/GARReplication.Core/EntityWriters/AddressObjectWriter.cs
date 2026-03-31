using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public class AddressObjectWriter: IEntityWriter
    {
        public string InsertQuery => """
                                        COPY address_objects
                                            (id, objectid, objectguid, changeid, name, typename,
                                            level, opertypeid, previd, nextid, updatedate, 
                                            startdate, enddate, isactual, isactive)
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public string FilePattern => @"AS_ADDR_OBJ_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

        public string NodeName => "OBJECT";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((long)entity.Attribute(_IDName), NpgsqlDbType.Bigint);
            writer.Write((long)entity.Attribute(_OBJECTIDName), NpgsqlDbType.Bigint);
            writer.Write((string)entity.Attribute(_OBJECTGUIDName), NpgsqlDbType.Varchar);
            writer.Write((long)entity.Attribute(_CHANGEIDName), NpgsqlDbType.Bigint);
            writer.Write((string)entity.Attribute(_NAMEName), NpgsqlDbType.Varchar);
            writer.Write((string)entity.Attribute(_TYPENAMEName), NpgsqlDbType.Varchar);
            writer.Write((string)entity.Attribute(_LEVELName), NpgsqlDbType.Varchar);
            writer.Write((int)entity.Attribute(_OPERTYPEIDName), NpgsqlDbType.Integer);
            writer.Write((long?)entity.Attribute(_PREVIDName), NpgsqlDbType.Bigint);
            writer.Write((long?)entity.Attribute(_NEXTIDName), NpgsqlDbType.Bigint);
            writer.Write((DateTime)entity.Attribute(_UPDATEDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_STARTDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_ENDDATEName), NpgsqlDbType.Date);
            writer.Write((int)entity.Attribute(_ISACTUALName), NpgsqlDbType.Integer);
            writer.Write((int)entity.Attribute(_ISACTIVEName), NpgsqlDbType.Integer);
            
        }
    }
}
