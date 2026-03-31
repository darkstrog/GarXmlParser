using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public class MunHierarchyWriter: IEntityWriter
    {
        public string InsertQuery => """
                                        COPY mun_hierarchy
                                            (id, objectid, parentobjid, changeid, 
                                            oktmo, previd, nextid, updatedate, startdate, 
                                            enddate, isactive, path)
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public string FilePattern => @"AS_MUN_HIERARCHY_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

        public string NodeName => "ITEM";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((long)entity.Attribute(_IDName), NpgsqlDbType.Bigint);
            writer.Write((long)entity.Attribute(_OBJECTIDName), NpgsqlDbType.Bigint);
            writer.Write((long?)entity.Attribute(_PARENTOBJIDName), NpgsqlDbType.Bigint);
            writer.Write((long)entity.Attribute(_CHANGEIDName), NpgsqlDbType.Bigint);
            writer.Write((string?)entity.Attribute(_OKTMOName), NpgsqlDbType.Varchar);
            writer.Write((long?)entity.Attribute(_PREVIDName), NpgsqlDbType.Bigint);
            writer.Write((long?)entity.Attribute(_NEXTIDName), NpgsqlDbType.Bigint);
            writer.Write((DateTime)entity.Attribute(_UPDATEDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_STARTDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_ENDDATEName), NpgsqlDbType.Date);
            writer.Write((int)entity.Attribute(_ISACTIVEName), NpgsqlDbType.Integer);
            writer.Write((string)entity.Attribute(_PATHName), NpgsqlDbType.Varchar);

        }
    }
}
