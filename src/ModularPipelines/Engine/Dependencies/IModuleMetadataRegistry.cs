using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Registry for resolved module metadata.
/// </summary>
internal interface IModuleMetadataRegistry : IDependencyContext
{
    void SetMetadata(Type moduleType, string key, object value);

    T? GetMetadata<T>(Type moduleType, string key);

    /// <summary>
    /// Finalizes metadata for a module after it's instantiated.
    /// Merges tags and categories from attributes and module configuration.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="instance">The module instance.</param>
    void FinalizeMetadata(Type moduleType, IModule instance);

    void CopyRegistrationMetadataTo(IModuleMetadataRegistry destination);
}
