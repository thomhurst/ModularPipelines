using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "export", "destination", "update")]
public record AzIotCentralExportDestinationUpdateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--content")] string Content,
[property: CliOption("--dest-id")] string DestId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}