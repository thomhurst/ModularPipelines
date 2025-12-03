using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "operations", "list")]
public record GcloudEndpointsOperationsListOptions : GcloudOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }
}