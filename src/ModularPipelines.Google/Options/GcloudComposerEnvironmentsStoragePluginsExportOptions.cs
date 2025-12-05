using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "storage", "plugins", "export")]
public record GcloudComposerEnvironmentsStoragePluginsExportOptions(
[property: CliOption("--destination")] string Destination,
[property: CliOption("--environment")] string Environment,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliOption("--source")]
    public string? Source { get; set; }
}