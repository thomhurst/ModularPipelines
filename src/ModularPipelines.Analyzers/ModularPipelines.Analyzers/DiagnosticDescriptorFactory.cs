using Microsoft.CodeAnalysis;

namespace ModularPipelines.Analyzers;

/// <summary>
/// Factory class for creating <see cref="DiagnosticDescriptor"/> instances with consistent configuration.
/// Eliminates boilerplate code across analyzers by centralizing the creation of localizable diagnostic descriptors.
/// </summary>
internal static class DiagnosticDescriptorFactory
{
    /// <summary>
    /// Creates a <see cref="DiagnosticDescriptor"/> with localizable strings from the Resources file.
    /// </summary>
    /// <param name="id">The unique identifier for the diagnostic.</param>
    /// <param name="titleResourceName">The resource name for the title string.</param>
    /// <param name="messageFormatResourceName">The resource name for the message format string.</param>
    /// <param name="descriptionResourceName">The resource name for the description string.</param>
    /// <param name="category">The category of the diagnostic. Defaults to "Usage".</param>
    /// <param name="severity">The severity of the diagnostic. Defaults to <see cref="DiagnosticSeverity.Error"/>.</param>
    /// <returns>A configured <see cref="DiagnosticDescriptor"/> instance.</returns>
    public static DiagnosticDescriptor Create(
        string id,
        string titleResourceName,
        string messageFormatResourceName,
        string descriptionResourceName,
        string category = "Usage",
        DiagnosticSeverity severity = DiagnosticSeverity.Error)
    {
        var title = new LocalizableResourceString(titleResourceName, Resources.ResourceManager, typeof(Resources));
        var messageFormat = new LocalizableResourceString(messageFormatResourceName, Resources.ResourceManager, typeof(Resources));
        var description = new LocalizableResourceString(descriptionResourceName, Resources.ResourceManager, typeof(Resources));

        return new DiagnosticDescriptor(
            id,
            title,
            messageFormat,
            category,
            severity,
            isEnabledByDefault: true,
            description: description);
    }
}
