using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-meetings", "delete-meeting")]
public record AwsChimeSdkMeetingsDeleteMeetingOptions(
[property: CommandSwitch("--meeting-id")] string MeetingId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}