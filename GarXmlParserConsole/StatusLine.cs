namespace GarXmlParserConsole
{
    public class StatusLine
    {
        private int statusLineRow;

        public StatusLine()
        {
            statusLineRow = Console.CursorTop;
            Console.WriteLine();
        }

        public void Update(string message)
        {
            int currentLine = Console.CursorTop;
            int currentColumn = Console.CursorLeft;

            Console.SetCursorPosition(0, statusLineRow);
            Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, statusLineRow);
            Console.Write(message);

            Console.SetCursorPosition(currentColumn, currentLine);
        }
    }

}
