using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public class ParamTypeWriter: IEntityWriter
    {
        public string InsertQuery => """
                                        COPY param_types
                                            (id, name, code, description, updatedate, 
                                            startdate, enddate, isactive)
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public string FilePattern => @"AS_PARAM_TYPES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

        public string NodeName => "PARAMTYPE";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((long)entity.Attribute(_IDName), NpgsqlDbType.Integer);
            writer.Write((string)entity.Attribute(_NAMEName), NpgsqlDbType.Varchar);
            writer.Write((string)entity.Attribute(_CODEName), NpgsqlDbType.Varchar);
            writer.Write((string)entity.Attribute(_DESCName), NpgsqlDbType.Varchar);
            writer.Write((DateTime)entity.Attribute(_UPDATEDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_STARTDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime)entity.Attribute(_ENDDATEName), NpgsqlDbType.Date);
            writer.Write((bool)entity.Attribute(_ISACTIVEName), NpgsqlDbType.Boolean);

        }
    }
}
