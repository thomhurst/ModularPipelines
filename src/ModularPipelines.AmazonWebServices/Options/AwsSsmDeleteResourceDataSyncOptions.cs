using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "delete-resource-data-sync")]
public record AwsSsmDeleteResourceDataSyncOptions(
[property: CommandSwitch("--sync-name")] string SyncName
) : AwsOptions
{
    [CommandSwitch("--sync-type")]
    public string? SyncType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}