namespace GAROperations.Core.Interfaces.Mappers
{
    public interface IDtoMapper<TSource, TDestination>
    {
        TDestination MapToDto(TSource source);
        //IAsyncEnumerable<TDestination> MapToDtoAsync(IAsyncEnumerable<TSource> sources, CancellationToken cancellationToken);
    }
}
