using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "api-token", "delete")]
public record AzIotCentralApiTokenDeleteOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--tkid")] string Tkid
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}