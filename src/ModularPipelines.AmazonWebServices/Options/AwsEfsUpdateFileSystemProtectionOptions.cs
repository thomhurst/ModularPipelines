using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "update-file-system-protection")]
public record AwsEfsUpdateFileSystemProtectionOptions(
[property: CliOption("--file-system-id")] string FileSystemId
) : AwsOptions
{
    [CliOption("--replication-overwrite-protection")]
    public string? ReplicationOverwriteProtection { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}