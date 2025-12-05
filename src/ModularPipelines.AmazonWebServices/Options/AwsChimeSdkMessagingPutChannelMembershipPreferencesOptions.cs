using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "put-channel-membership-preferences")]
public record AwsChimeSdkMessagingPutChannelMembershipPreferencesOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--member-arn")] string MemberArn,
[property: CliOption("--chime-bearer")] string ChimeBearer,
[property: CliOption("--preferences")] string Preferences
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}