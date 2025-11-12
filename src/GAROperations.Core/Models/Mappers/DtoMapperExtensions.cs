using GAROperations.Core.Interfaces.Mappers;
using System.Runtime.CompilerServices;

namespace GAROperations.Core.Models.Mappers
{
    /// <summary>
    /// Метод расширение для DTO мапперов 
    /// </summary>
    public static class DtoMapperExtensions
    {
        public static async IAsyncEnumerable<TDto> MapToDtoAsync<TEntity, TDto>(
                this IDtoMapper<TEntity, TDto> mapper,
                IAsyncEnumerable<TEntity> sources,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
                where TDto : class
                where TEntity : class
        {
            await foreach (var source in sources.WithCancellation(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                TDto? result;
                try
                {
                    result = mapper.MapToDto(source);
                }
                catch (Exception)
                {
                    continue;
                }

                if (result != null)
                    yield return result;
            }
        }
    }
}
