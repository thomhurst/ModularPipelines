using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs", "start-viewer-session-revocation")]
public record AwsIvsStartViewerSessionRevocationOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--viewer-id")] string ViewerId
) : AwsOptions
{
    [CliOption("--viewer-session-versions-less-than-or-equal-to")]
    public int? ViewerSessionVersionsLessThanOrEqualTo { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}