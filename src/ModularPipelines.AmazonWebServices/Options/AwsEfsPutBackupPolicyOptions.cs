using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "put-backup-policy")]
public record AwsEfsPutBackupPolicyOptions(
[property: CliOption("--file-system-id")] string FileSystemId,
[property: CliOption("--backup-policy")] string BackupPolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}