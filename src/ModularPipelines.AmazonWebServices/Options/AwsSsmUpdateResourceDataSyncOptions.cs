using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-resource-data-sync")]
public record AwsSsmUpdateResourceDataSyncOptions(
[property: CommandSwitch("--sync-name")] string SyncName,
[property: CommandSwitch("--sync-type")] string SyncType,
[property: CommandSwitch("--sync-source")] string SyncSource
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}