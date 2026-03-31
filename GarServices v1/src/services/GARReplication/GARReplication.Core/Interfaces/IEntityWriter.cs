using Npgsql;
using System.Xml.Linq;

namespace GARReplication.Core.Interfaces
{
    public interface IEntityWriter
    {
        string InsertQuery { get; }
        string FilePattern { get; }
        string NodeName {  get; }
        public void WriteRow(NpgsqlBinaryImporter writer, XElement entity);
    }
}
