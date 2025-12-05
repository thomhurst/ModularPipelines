using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudhsmv2", "copy-backup-to-region")]
public record AwsCloudhsmv2CopyBackupToRegionOptions(
[property: CliOption("--destination-region")] string DestinationRegion,
[property: CliOption("--backup-id")] string BackupId
) : AwsOptions
{
    [CliOption("--tag-list")]
    public string[]? TagList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}