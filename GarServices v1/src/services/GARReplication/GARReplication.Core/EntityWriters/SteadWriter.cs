using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public class SteadWriter: IEntityWriter
    {
        public string InsertQuery => """
                                        COPY stead
                                            (id, objectid, objectguid, changeid, steadnumber,
                                            opertypeid, previd, nextid, updatedate, startdate,
                                            enddate, isactual, isactive)
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public string FilePattern => @"AS_STEADS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

        public string NodeName => "STEAD";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((int)entity.Attribute(_IDName), NpgsqlDbType.Integer);
            writer.Write((int)entity.Attribute(_OBJECTIDName), NpgsqlDbType.Integer);
            writer.Write((string)entity.Attribute(_OBJECTGUIDName), NpgsqlDbType.Varchar);
            writer.Write((int)entity.Attribute(_CHANGEIDName), NpgsqlDbType.Integer);
            writer.Write((string)entity.Attribute(_NUMBERName), NpgsqlDbType.Varchar);
            writer.Write((string)entity.Attribute(_OPERTYPEIDName), NpgsqlDbType.Varchar);
            writer.Write((int?)entity.Attribute(_PREVIDName), NpgsqlDbType.Integer);
            writer.Write((int?)entity.Attribute(_NEXTIDName), NpgsqlDbType.Integer);
            writer.Write((DateTime)entity.Attribute(_UPDATEDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_STARTDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_ENDDATEName), NpgsqlDbType.Date);
            writer.Write((int)entity.Attribute(_ISACTUALName), NpgsqlDbType.Integer);
            writer.Write((int)entity.Attribute(_ISACTIVEName), NpgsqlDbType.Integer);

        }
    }
}
