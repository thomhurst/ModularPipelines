using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("efs", "put-backup-policy")]
public record AwsEfsPutBackupPolicyOptions(
[property: CommandSwitch("--file-system-id")] string FileSystemId,
[property: CommandSwitch("--backup-policy")] string BackupPolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}