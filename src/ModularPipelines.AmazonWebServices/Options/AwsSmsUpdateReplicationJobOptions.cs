using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sms", "update-replication-job")]
public record AwsSmsUpdateReplicationJobOptions(
[property: CliOption("--replication-job-id")] string ReplicationJobId
) : AwsOptions
{
    [CliOption("--frequency")]
    public int? Frequency { get; set; }

    [CliOption("--next-replication-run-start-time")]
    public long? NextReplicationRunStartTime { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--role-name")]
    public string? RoleName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--number-of-recent-amis-to-keep")]
    public int? NumberOfRecentAmisToKeep { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}