using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "job", "rerun")]
public record AzIotCentralJobRerunOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--rerun-id")] string RerunId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}