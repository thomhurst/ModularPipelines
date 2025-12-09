using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cdn", "origin-group", "create")]
public record AzCdnOriginGroupCreateOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--name")] string Name,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
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
}