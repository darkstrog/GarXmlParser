namespace GarXmlParser
{
    public record ProcessingProgress(
        int CurrentFileIndex,
        int TotalFiles,
        string CurrentFilePath,
        int TotalItemsProcessed,
        int failedItems
     );
}
