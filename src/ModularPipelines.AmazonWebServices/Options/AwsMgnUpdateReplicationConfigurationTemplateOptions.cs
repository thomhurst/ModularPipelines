using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "update-replication-configuration-template")]
public record AwsMgnUpdateReplicationConfigurationTemplateOptions(
[property: CommandSwitch("--replication-configuration-template-id")] string ReplicationConfigurationTemplateId
) : AwsOptions
{
    [CommandSwitch("--arn")]
    public string? Arn { get; set; }

    [CommandSwitch("--bandwidth-throttling")]
    public long? BandwidthThrottling { get; set; }

    [CommandSwitch("--data-plane-routing")]
    public string? DataPlaneRouting { get; set; }

    [CommandSwitch("--default-large-staging-disk-type")]
    public string? DefaultLargeStagingDiskType { get; set; }

    [CommandSwitch("--ebs-encryption")]
    public string? EbsEncryption { get; set; }

    [CommandSwitch("--ebs-encryption-key-arn")]
    public string? EbsEncryptionKeyArn { get; set; }

    [CommandSwitch("--replication-server-instance-type")]
    public string? ReplicationServerInstanceType { get; set; }

    [CommandSwitch("--replication-servers-security-groups-ids")]
    public string[]? ReplicationServersSecurityGroupsIds { get; set; }

    [CommandSwitch("--staging-area-subnet-id")]
    public string? StagingAreaSubnetId { get; set; }

    [CommandSwitch("--staging-area-tags")]
    public IEnumerable<KeyValue>? StagingAreaTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}