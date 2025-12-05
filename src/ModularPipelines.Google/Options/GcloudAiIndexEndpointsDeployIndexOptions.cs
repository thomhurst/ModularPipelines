using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "index-endpoints", "deploy-index")]
public record GcloudAiIndexEndpointsDeployIndexOptions(
[property: CliArgument] string IndexEndpoint,
[property: CliArgument] string Region,
[property: CliOption("--deployed-index-id")] string DeployedIndexId,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--index")] string Index
) : GcloudOptions
{
    [CliOption("--allowed-issuers")]
    public string[]? AllowedIssuers { get; set; }

    [CliOption("--audiences")]
    public string[]? Audiences { get; set; }

    [CliOption("--deployment-group")]
    public string? DeploymentGroup { get; set; }

    [CliFlag("--enable-access-logging")]
    public bool? EnableAccessLogging { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--max-replica-count")]
    public string? MaxReplicaCount { get; set; }

    [CliOption("--min-replica-count")]
    public string? MinReplicaCount { get; set; }

    [CliOption("--reserved-ip-ranges")]
    public string[]? ReservedIpRanges { get; set; }
}