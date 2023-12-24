using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "create-replication-configuration-template")]
public record AwsMgnCreateReplicationConfigurationTemplateOptions(
[property: CommandSwitch("--bandwidth-throttling")] long BandwidthThrottling,
[property: CommandSwitch("--data-plane-routing")] string DataPlaneRouting,
[property: CommandSwitch("--default-large-staging-disk-type")] string DefaultLargeStagingDiskType,
[property: CommandSwitch("--ebs-encryption")] string EbsEncryption,
[property: CommandSwitch("--replication-server-instance-type")] string ReplicationServerInstanceType,
[property: CommandSwitch("--replication-servers-security-groups-ids")] string[] ReplicationServersSecurityGroupsIds,
[property: CommandSwitch("--staging-area-subnet-id")] string StagingAreaSubnetId,
[property: CommandSwitch("--staging-area-tags")] IEnumerable<KeyValue> StagingAreaTags
) : AwsOptions
{
    [CommandSwitch("--ebs-encryption-key-arn")]
    public string? EbsEncryptionKeyArn { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}