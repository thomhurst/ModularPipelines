using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sms", "create-replication-job")]
public record AwsSmsCreateReplicationJobOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--seed-replication-time")] long SeedReplicationTime
) : AwsOptions
{
    [CliOption("--frequency")]
    public int? Frequency { get; set; }

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