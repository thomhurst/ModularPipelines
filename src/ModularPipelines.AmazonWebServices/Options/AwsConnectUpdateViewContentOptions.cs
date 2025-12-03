using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-view-content")]
public record AwsConnectUpdateViewContentOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--view-id")] string ViewId,
[property: CliOption("--status")] string Status,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}