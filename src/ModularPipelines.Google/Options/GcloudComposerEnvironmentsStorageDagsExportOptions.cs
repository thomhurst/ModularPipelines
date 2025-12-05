using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "storage", "dags", "export")]
public record GcloudComposerEnvironmentsStorageDagsExportOptions(
[property: CliOption("--destination")] string Destination,
[property: CliOption("--environment")] string Environment,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliOption("--source")]
    public string? Source { get; set; }
}