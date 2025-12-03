using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ids", "endpoints", "delete")]
public record GcloudIdsEndpointsDeleteOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--max-wait")]
    public string? MaxWait { get; set; }
}