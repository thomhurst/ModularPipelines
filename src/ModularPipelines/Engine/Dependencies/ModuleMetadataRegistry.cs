using System.Collections.Concurrent;

namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Stores metadata for modules set during registration.
/// </summary>
internal class ModuleMetadataRegistry : IModuleMetadataRegistry
{
    private readonly ConcurrentDictionary<(Type, string), object> _metadata = new();

    public void SetMetadata(Type moduleType, string key, object value)
    {
        _metadata[(moduleType, key)] = value;
    }

    public T? GetMetadata<T>(Type moduleType, string key)
    {
        if (_metadata.TryGetValue((moduleType, key), out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }
}
