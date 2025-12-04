using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "export", "create")]
public record AzIotCentralExportCreateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--destinations")] string Destinations,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--export-id")] string ExportId,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--en")]
    public string? En { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}