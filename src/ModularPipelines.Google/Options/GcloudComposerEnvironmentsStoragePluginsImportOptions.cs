using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "storage", "plugins", "import")]
public record GcloudComposerEnvironmentsStoragePluginsImportOptions(
[property: CliOption("--source")] string Source,
[property: CliOption("--environment")] string Environment,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}