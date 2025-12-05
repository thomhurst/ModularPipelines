using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "export", "destination", "show")]
public record AzIotCentralExportDestinationShowOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--dest-id")] string DestId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}