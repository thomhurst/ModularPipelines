using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "index-endpoints", "deploy-index")]
public record GcloudAiIndexEndpointsDeployIndexOptions(
[property: PositionalArgument] string IndexEndpoint,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--deployed-index-id")] string DeployedIndexId,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--index")] string Index
) : GcloudOptions
{
    [CommandSwitch("--allowed-issuers")]
    public string[]? AllowedIssuers { get; set; }

    [CommandSwitch("--audiences")]
    public string[]? Audiences { get; set; }

    [CommandSwitch("--deployment-group")]
    public string? DeploymentGroup { get; set; }

    [BooleanCommandSwitch("--enable-access-logging")]
    public bool? EnableAccessLogging { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--max-replica-count")]
    public string? MaxReplicaCount { get; set; }

    [CommandSwitch("--min-replica-count")]
    public string? MinReplicaCount { get; set; }

    [CommandSwitch("--reserved-ip-ranges")]
    public string[]? ReservedIpRanges { get; set; }
}