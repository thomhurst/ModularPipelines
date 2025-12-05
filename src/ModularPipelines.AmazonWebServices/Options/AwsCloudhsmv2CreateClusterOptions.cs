using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudhsmv2", "create-cluster")]
public record AwsCloudhsmv2CreateClusterOptions(
[property: CliOption("--hsm-type")] string HsmType,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--backup-retention-policy")]
    public string? BackupRetentionPolicy { get; set; }

    [CliOption("--source-backup-id")]
    public string? SourceBackupId { get; set; }

    [CliOption("--tag-list")]
    public string[]? TagList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}