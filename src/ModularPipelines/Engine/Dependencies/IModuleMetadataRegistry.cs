using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Registry for module metadata set during registration.
/// </summary>
internal interface IModuleMetadataRegistry : IDependencyContext
{
    void SetMetadata(Type moduleType, string key, object value);

    T? GetMetadata<T>(Type moduleType, string key);

    /// <summary>
    /// Adds tags to a module during registration.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="tags">The tags to add.</param>
    void AddRegistrationTags(Type moduleType, IEnumerable<string> tags);

    /// <summary>
    /// Sets the category for a module during registration.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="category">The category to set.</param>
    void SetRegistrationCategory(Type moduleType, string category);

    /// <summary>
    /// Finalizes metadata for a module after it's instantiated.
    /// Merges tags and categories from attributes, instance overrides, and registration-time configuration.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="instance">The module instance.</param>
    void FinalizeMetadata(Type moduleType, IModule instance);
}
