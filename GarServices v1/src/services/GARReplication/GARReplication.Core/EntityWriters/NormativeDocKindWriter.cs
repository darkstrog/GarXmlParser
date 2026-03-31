using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public class NormativeDocKindWriter: IEntityWriter
    {
        public string InsertQuery => """
                                        COPY normative_doc_kind
                                            (id, name)
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public string FilePattern => @"AS_NORMATIVE_DOCS_KINDS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

        public string NodeName => "NDOCKIND";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((int)entity.Attribute(_IDName), NpgsqlDbType.Integer);
            writer.Write((string)entity.Attribute(_NAMEName), NpgsqlDbType.Varchar);

        }
    }
}
