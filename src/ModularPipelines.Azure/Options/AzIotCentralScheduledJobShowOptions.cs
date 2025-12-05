using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "scheduled-job", "show")]
public record AzIotCentralScheduledJobShowOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}