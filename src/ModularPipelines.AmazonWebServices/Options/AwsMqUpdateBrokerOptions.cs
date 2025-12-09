using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mq", "update-broker")]
public record AwsMqUpdateBrokerOptions(
[property: CliOption("--broker-id")] string BrokerId
) : AwsOptions
{
    [CliOption("--authentication-strategy")]
    public string? AuthenticationStrategy { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--host-instance-type")]
    public string? HostInstanceType { get; set; }

    [CliOption("--ldap-server-metadata")]
    public string? LdapServerMetadata { get; set; }

    [CliOption("--logs")]
    public string? Logs { get; set; }

    [CliOption("--maintenance-window-start-time")]
    public string? MaintenanceWindowStartTime { get; set; }

    [CliOption("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CliOption("--data-replication-mode")]
    public string? DataReplicationMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}