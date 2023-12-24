using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackage-vod", "create-asset")]
public record AwsMediapackageVodCreateAssetOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--packaging-group-id")] string PackagingGroupId,
[property: CommandSwitch("--source-arn")] string SourceArn,
[property: CommandSwitch("--source-role-arn")] string SourceRoleArn
) : AwsOptions
{
    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}