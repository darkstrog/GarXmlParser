using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public class AddressObjectTypeWriter: IEntityWriter
    {
        public string InsertQuery => """
                                        COPY address_object_types
                                            (id, level, shortname, name, description, 
                                            updatedate, startdate, enddate, isactive)
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public string FilePattern => @"AS_ADDR_OBJ_TYPES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

        public string NodeName => "ADDRESSOBJECTTYPE";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((int)entity.Attribute(_IDName), NpgsqlDbType.Integer);
            writer.Write((int)entity.Attribute(_LEVELName), NpgsqlDbType.Integer);
            writer.Write((string)entity.Attribute(_SHORTNAMEName), NpgsqlDbType.Varchar);
            writer.Write((string)entity.Attribute(_NAMEName), NpgsqlDbType.Varchar);
            writer.Write((string?)entity.Attribute(_DESCName), NpgsqlDbType.Varchar);
            writer.Write((DateTime)entity.Attribute(_UPDATEDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_STARTDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_ENDDATEName), NpgsqlDbType.Date);
            writer.Write((bool)entity.Attribute(_ISACTIVEName), NpgsqlDbType.Boolean);
        }
    }
}
