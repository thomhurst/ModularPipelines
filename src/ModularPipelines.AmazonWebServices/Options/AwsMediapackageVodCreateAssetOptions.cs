using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackage-vod", "create-asset")]
public record AwsMediapackageVodCreateAssetOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--packaging-group-id")] string PackagingGroupId,
[property: CliOption("--source-arn")] string SourceArn,
[property: CliOption("--source-role-arn")] string SourceRoleArn
) : AwsOptions
{
    [CliOption("--resource-id")]
    public string? ResourceId { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}