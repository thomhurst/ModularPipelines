namespace ModularPipelines.Modules;

/// <summary>
/// Interface for modules that declare tags and/or category programmatically.
/// </summary>
/// <remarks>
/// Implement this interface or override the virtual properties in <see cref="Module{T}"/>
/// to declare tags and categories at runtime rather than via attributes.
/// </remarks>
public interface ITaggedModule
{
    /// <summary>
    /// Gets the tags for this module. Tags describe characteristics of the module.
    /// </summary>
    IReadOnlySet<string> Tags { get; }

    /// <summary>
    /// Gets the category for this module. Categories identify what the module is.
    /// </summary>
    string? Category { get; }
}
