using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "put-events")]
public record AwsPinpointPutEventsOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--events-request")] string EventsRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}