using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "update-replication-configuration-template")]
public record AwsDrsUpdateReplicationConfigurationTemplateOptions(
[property: CliOption("--replication-configuration-template-id")] string ReplicationConfigurationTemplateId
) : AwsOptions
{
    [CliOption("--arn")]
    public string? Arn { get; set; }

    [CliOption("--bandwidth-throttling")]
    public long? BandwidthThrottling { get; set; }

    [CliOption("--data-plane-routing")]
    public string? DataPlaneRouting { get; set; }

    [CliOption("--default-large-staging-disk-type")]
    public string? DefaultLargeStagingDiskType { get; set; }

    [CliOption("--ebs-encryption")]
    public string? EbsEncryption { get; set; }

    [CliOption("--ebs-encryption-key-arn")]
    public string? EbsEncryptionKeyArn { get; set; }

    [CliOption("--pit-policy")]
    public string[]? PitPolicy { get; set; }

    [CliOption("--replication-server-instance-type")]
    public string? ReplicationServerInstanceType { get; set; }

    [CliOption("--replication-servers-security-groups-ids")]
    public string[]? ReplicationServersSecurityGroupsIds { get; set; }

    [CliOption("--staging-area-subnet-id")]
    public string? StagingAreaSubnetId { get; set; }

    [CliOption("--staging-area-tags")]
    public IEnumerable<KeyValue>? StagingAreaTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}