using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "send-test-event-notification")]
public record AwsMturkSendTestEventNotificationOptions(
[property: CommandSwitch("--notification")] string Notification,
[property: CommandSwitch("--test-event-type")] string TestEventType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}