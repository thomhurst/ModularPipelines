using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "list-channel-memberships-for-app-instance-user")]
public record AwsChimeSdkMessagingListChannelMembershipsForAppInstanceUserOptions(
[property: CommandSwitch("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CommandSwitch("--app-instance-user-arn")]
    public string? AppInstanceUserArn { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}