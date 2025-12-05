using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mq", "create-broker")]
public record AwsMqCreateBrokerOptions(
[property: CliOption("--broker-name")] string BrokerName,
[property: CliOption("--deployment-mode")] string DeploymentMode,
[property: CliOption("--engine-type")] string EngineType,
[property: CliOption("--engine-version")] string EngineVersion,
[property: CliOption("--host-instance-type")] string HostInstanceType,
[property: CliOption("--users")] string[] Users
) : AwsOptions
{
    [CliOption("--authentication-strategy")]
    public string? AuthenticationStrategy { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--encryption-options")]
    public string? EncryptionOptions { get; set; }

    [CliOption("--ldap-server-metadata")]
    public string? LdapServerMetadata { get; set; }

    [CliOption("--logs")]
    public string? Logs { get; set; }

    [CliOption("--maintenance-window-start-time")]
    public string? MaintenanceWindowStartTime { get; set; }

    [CliOption("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--data-replication-mode")]
    public string? DataReplicationMode { get; set; }

    [CliOption("--data-replication-primary-broker-arn")]
    public string? DataReplicationPrimaryBrokerArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}