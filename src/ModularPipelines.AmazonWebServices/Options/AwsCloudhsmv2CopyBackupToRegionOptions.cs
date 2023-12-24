using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudhsmv2", "copy-backup-to-region")]
public record AwsCloudhsmv2CopyBackupToRegionOptions(
[property: CommandSwitch("--destination-region")] string DestinationRegion,
[property: CommandSwitch("--backup-id")] string BackupId
) : AwsOptions
{
    [CommandSwitch("--tag-list")]
    public string[]? TagList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}