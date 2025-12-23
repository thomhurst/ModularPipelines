namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Registry for module metadata set during registration.
/// </summary>
internal interface IModuleMetadataRegistry
{
    void SetMetadata(Type moduleType, string key, object value);

    T? GetMetadata<T>(Type moduleType, string key);
}
