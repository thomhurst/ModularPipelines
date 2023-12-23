using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs", "start-viewer-session-revocation")]
public record AwsIvsStartViewerSessionRevocationOptions(
[property: CommandSwitch("--channel-arn")] string ChannelArn,
[property: CommandSwitch("--viewer-id")] string ViewerId
) : AwsOptions
{
    [CommandSwitch("--viewer-session-versions-less-than-or-equal-to")]
    public int? ViewerSessionVersionsLessThanOrEqualTo { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}