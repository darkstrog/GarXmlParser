using GARReplication.Commands;

namespace GARReplication
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await CommandLineArgsParser.ParseAsync(args);
        }
        
    }
}