using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudhsmv2", "create-cluster")]
public record AwsCloudhsmv2CreateClusterOptions(
[property: CommandSwitch("--hsm-type")] string HsmType,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--backup-retention-policy")]
    public string? BackupRetentionPolicy { get; set; }

    [CommandSwitch("--source-backup-id")]
    public string? SourceBackupId { get; set; }

    [CommandSwitch("--tag-list")]
    public string[]? TagList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}