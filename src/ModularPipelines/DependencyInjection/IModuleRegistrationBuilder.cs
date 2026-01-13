using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.DependencyInjection;

/// <summary>
/// Fluent builder for configuring module registration with metadata.
/// </summary>
public interface IModuleRegistrationBuilder
{
    /// <summary>
    /// Gets the service collection for continued registration.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Adds tags to the module.
    /// </summary>
    /// <param name="tags">The tags to add.</param>
    /// <returns>The builder for chaining.</returns>
    IModuleRegistrationBuilder WithTags(params string[] tags);

    /// <summary>
    /// Sets the category of the module.
    /// </summary>
    /// <param name="category">The category to set.</param>
    /// <returns>The builder for chaining.</returns>
    IModuleRegistrationBuilder WithCategory(string category);
}
