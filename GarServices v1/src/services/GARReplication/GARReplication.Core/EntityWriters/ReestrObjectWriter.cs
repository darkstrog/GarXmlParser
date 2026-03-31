using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public class ReestrObjectWriter: IEntityWriter
    {
        public string InsertQuery => """
                                        COPY reestr_object
                                            (objectid, createdate, changeid, levelid,
                                            updatedate, objectguid, isactive)
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public string FilePattern => @"AS_REESTR_OBJECTS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

        public string NodeName => "OBJECT";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((long)entity.Attribute(_OBJECTIDName), NpgsqlDbType.Bigint);
            writer.Write((DateTime)entity.Attribute(_CREATEDATEName), NpgsqlDbType.Date);
            writer.Write((long)entity.Attribute(_CHANGEIDName), NpgsqlDbType.Bigint);
            writer.Write((int)entity.Attribute(_LEVELIDName), NpgsqlDbType.Integer);
            writer.Write((DateTime)entity.Attribute(_UPDATEDATEName), NpgsqlDbType.Date);
            writer.Write((string)entity.Attribute(_OBJECTGUIDName), NpgsqlDbType.Varchar);
            writer.Write((int)entity.Attribute(_ISACTIVEName), NpgsqlDbType.Integer);

        }
     
    }
}
