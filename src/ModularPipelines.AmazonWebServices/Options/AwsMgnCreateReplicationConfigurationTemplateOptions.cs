using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "create-replication-configuration-template")]
public record AwsMgnCreateReplicationConfigurationTemplateOptions(
[property: CliOption("--bandwidth-throttling")] long BandwidthThrottling,
[property: CliOption("--data-plane-routing")] string DataPlaneRouting,
[property: CliOption("--default-large-staging-disk-type")] string DefaultLargeStagingDiskType,
[property: CliOption("--ebs-encryption")] string EbsEncryption,
[property: CliOption("--replication-server-instance-type")] string ReplicationServerInstanceType,
[property: CliOption("--replication-servers-security-groups-ids")] string[] ReplicationServersSecurityGroupsIds,
[property: CliOption("--staging-area-subnet-id")] string StagingAreaSubnetId,
[property: CliOption("--staging-area-tags")] IEnumerable<KeyValue> StagingAreaTags
) : AwsOptions
{
    [CliOption("--ebs-encryption-key-arn")]
    public string? EbsEncryptionKeyArn { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}