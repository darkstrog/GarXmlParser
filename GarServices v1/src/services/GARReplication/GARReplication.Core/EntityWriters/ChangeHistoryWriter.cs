using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public class ChangeHistoryWriter: IEntityWriter
    {
        public string InsertQuery => """
                                        COPY change_history_item
                                            (changeid, objectid, adrobjectid,
                                            opertypeid, ndocid, changedate)
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public string FilePattern => @"AS_CHANGE_HISTORY_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

        public string NodeName => "ITEM";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((long)entity.Attribute(_CHANGEIDName), NpgsqlDbType.Bigint);
            writer.Write((long)entity.Attribute(_OBJECTIDName), NpgsqlDbType.Bigint);
            writer.Write((string)entity.Attribute(_ADROBJECTIDName), NpgsqlDbType.Varchar);
            writer.Write((int)entity.Attribute(_OPERTYPEIDName), NpgsqlDbType.Integer);
            writer.Write((long?)entity.Attribute(_NDOCIDName), NpgsqlDbType.Bigint);
            writer.Write((DateTime)entity.Attribute(_CHANGEDATEName), NpgsqlDbType.Date);
        }
    }
}
