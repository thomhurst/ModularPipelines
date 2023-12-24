using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sms", "create-replication-job")]
public record AwsSmsCreateReplicationJobOptions(
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--seed-replication-time")] long SeedReplicationTime
) : AwsOptions
{
    [CommandSwitch("--frequency")]
    public int? Frequency { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--role-name")]
    public string? RoleName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--number-of-recent-amis-to-keep")]
    public int? NumberOfRecentAmisToKeep { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}