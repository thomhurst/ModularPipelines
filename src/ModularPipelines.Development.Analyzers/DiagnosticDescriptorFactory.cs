using Microsoft.CodeAnalysis;

namespace ModularPipelines.Development.Analyzers;

internal static class DiagnosticDescriptorFactory
{
    private const string DocumentationBaseUrl =
        "https://thomhurst.github.io/ModularPipelines/docs/next/analyzers/";

    public static DiagnosticDescriptor Create(
        string id,
        string titleResourceName,
        string messageFormatResourceName,
        string descriptionResourceName)
    {
        var title = new LocalizableResourceString(
            titleResourceName,
            Resources.ResourceManager,
            typeof(Resources));
        var messageFormat = new LocalizableResourceString(
            messageFormatResourceName,
            Resources.ResourceManager,
            typeof(Resources));
        var description = new LocalizableResourceString(
            descriptionResourceName,
            Resources.ResourceManager,
            typeof(Resources));

        return new DiagnosticDescriptor(
            id,
            title,
            messageFormat,
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: description,
            helpLinkUri: DocumentationBaseUrl + id);
    }
}
