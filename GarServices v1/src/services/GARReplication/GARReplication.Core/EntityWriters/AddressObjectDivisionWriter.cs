using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public class AddressObjectDivisionWriter: IEntityWriter
    {
        public string InsertQuery => """
                                        COPY change_history_item 
                                        (id, parentid, childid, changeid) 
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public string FilePattern => @"AS_ADDR_OBJ_DIVISION_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

        public string NodeName => "ITEM";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((long)entity.Attribute(_IDName), NpgsqlDbType.Bigint);
            writer.Write((long)entity.Attribute(_PARENTIDName), NpgsqlDbType.Bigint);
            writer.Write((long)entity.Attribute(_CHILDIDName), NpgsqlDbType.Bigint);
            writer.Write((long)entity.Attribute(_CHANGEIDName), NpgsqlDbType.Bigint);
        }
    }
}
