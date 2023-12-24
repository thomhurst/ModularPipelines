using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "create-resource-data-sync")]
public record AwsSsmCreateResourceDataSyncOptions(
[property: CommandSwitch("--sync-name")] string SyncName
) : AwsOptions
{
    [CommandSwitch("--s3-destination")]
    public string? S3Destination { get; set; }

    [CommandSwitch("--sync-type")]
    public string? SyncType { get; set; }

    [CommandSwitch("--sync-source")]
    public string? SyncSource { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}