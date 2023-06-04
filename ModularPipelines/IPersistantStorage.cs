using ModularPipelines.Models;

namespace ModularPipelines;

public interface IPersistantStorage<TStorageMetadata>
{
    public Task PersistResult<TResult>(TStorageMetadata storageMetadata, Type moduleType, ModuleResult<TResult> moduleResult);
}

public class PersistantStorage : IPersistantStorage<GitStorageMetadata>
{
    public Task PersistResult<TResult>(GitStorageMetadata storageMetadata, Type moduleType, ModuleResult<TResult> moduleResult)
    {
        throw new NotImplementedException();
    }
}

public record GitStorageMetadata
{
    public string? BranchName { get; init; }
}