using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "api-token", "show")]
public record AzIotCentralApiTokenShowOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--tkid")] string Tkid
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}