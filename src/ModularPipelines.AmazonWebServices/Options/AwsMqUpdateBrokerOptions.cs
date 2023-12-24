using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mq", "update-broker")]
public record AwsMqUpdateBrokerOptions(
[property: CommandSwitch("--broker-id")] string BrokerId
) : AwsOptions
{
    [CommandSwitch("--authentication-strategy")]
    public string? AuthenticationStrategy { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--host-instance-type")]
    public string? HostInstanceType { get; set; }

    [CommandSwitch("--ldap-server-metadata")]
    public string? LdapServerMetadata { get; set; }

    [CommandSwitch("--logs")]
    public string? Logs { get; set; }

    [CommandSwitch("--maintenance-window-start-time")]
    public string? MaintenanceWindowStartTime { get; set; }

    [CommandSwitch("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CommandSwitch("--data-replication-mode")]
    public string? DataReplicationMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}