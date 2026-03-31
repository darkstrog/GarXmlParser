using GARReplication.Core.Interfaces;
using Npgsql;
using NpgsqlTypes;
using System.Xml.Linq;
using static GARReplication.Core.EntityWriters.XmlAttributeNames; // ВНИМАНИЕ: Статический класс с именами атрибутов XML

namespace GARReplication.Core.EntityWriters
{
    public class NormativeDocWriter: IEntityWriter
    {
        public string InsertQuery => """
                                        COPY normative_doc
                                            (id, name, doc_date, doc_number, type, kind, 
                                            update_date, org_name, reg_num, reg_date, 
                                            acc_date, comment)
                                        FROM STDIN (FORMAT BINARY)
                                        """;

        public string FilePattern => @"AS_NORMATIVE_DOCS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

        public string NodeName => "NORMDOC";

        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity)
        {
            writer.StartRow();

            writer.Write((long)entity.Attribute(_IDName), NpgsqlDbType.Bigint);
            writer.Write((string)entity.Attribute(_NAMEName), NpgsqlDbType.Varchar);
            writer.Write((DateTime)entity.Attribute(_DATEName), NpgsqlDbType.Date);
            writer.Write((string)entity.Attribute(_NUMBERName), NpgsqlDbType.Varchar);
            writer.Write((int)entity.Attribute(_TYPEName), NpgsqlDbType.Integer);
            writer.Write((int)entity.Attribute(_KINDName), NpgsqlDbType.Integer);
            writer.Write((DateTime)entity.Attribute(_UPDATEDATEName), NpgsqlDbType.Date);
            writer.Write((string?)entity.Attribute(_ORGNAMEName), NpgsqlDbType.Varchar);
            writer.Write((string?)entity.Attribute(_REGNUMName), NpgsqlDbType.Varchar);
            writer.Write((DateTime?)entity.Attribute(_REGDATEName), NpgsqlDbType.Date);
            writer.Write((DateTime?)entity.Attribute(_ACCDATEName), NpgsqlDbType.Date);
            writer.Write((string?)entity.Attribute(_COMMENTName), NpgsqlDbType.Varchar);

        }
        
    }
}
