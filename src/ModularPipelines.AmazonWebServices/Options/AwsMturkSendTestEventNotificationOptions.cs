using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "send-test-event-notification")]
public record AwsMturkSendTestEventNotificationOptions(
[property: CliOption("--notification")] string Notification,
[property: CliOption("--test-event-type")] string TestEventType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}