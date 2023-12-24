using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mq", "create-broker")]
public record AwsMqCreateBrokerOptions(
[property: CommandSwitch("--broker-name")] string BrokerName,
[property: CommandSwitch("--deployment-mode")] string DeploymentMode,
[property: CommandSwitch("--engine-type")] string EngineType,
[property: CommandSwitch("--engine-version")] string EngineVersion,
[property: CommandSwitch("--host-instance-type")] string HostInstanceType,
[property: CommandSwitch("--users")] string[] Users
) : AwsOptions
{
    [CommandSwitch("--authentication-strategy")]
    public string? AuthenticationStrategy { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--encryption-options")]
    public string? EncryptionOptions { get; set; }

    [CommandSwitch("--ldap-server-metadata")]
    public string? LdapServerMetadata { get; set; }

    [CommandSwitch("--logs")]
    public string? Logs { get; set; }

    [CommandSwitch("--maintenance-window-start-time")]
    public string? MaintenanceWindowStartTime { get; set; }

    [CommandSwitch("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--data-replication-mode")]
    public string? DataReplicationMode { get; set; }

    [CommandSwitch("--data-replication-primary-broker-arn")]
    public string? DataReplicationPrimaryBrokerArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}