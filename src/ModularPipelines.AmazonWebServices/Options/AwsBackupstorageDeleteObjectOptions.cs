using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backupstorage", "delete-object")]
public record AwsBackupstorageDeleteObjectOptions(
[property: CliOption("--backup-job-id")] string BackupJobId,
[property: CliOption("--object-name")] string ObjectName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}