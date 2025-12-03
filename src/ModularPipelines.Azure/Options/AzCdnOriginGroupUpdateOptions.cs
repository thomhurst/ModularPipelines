using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cdn", "origin-group", "update")]
public record AzCdnOriginGroupUpdateOptions : AzOptions
{
    [CliOption("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--origins")]
    public string? Origins { get; set; }

    [CliOption("--probe-interval")]
    public string? ProbeInterval { get; set; }

    [CliOption("--probe-method")]
    public string? ProbeMethod { get; set; }

    [CliOption("--probe-path")]
    public string? ProbePath { get; set; }

    [CliOption("--probe-protocol")]
    public string? ProbeProtocol { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}