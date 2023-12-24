using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "put-event-stream")]
public record AwsPinpointPutEventStreamOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--write-event-stream")] string WriteEventStream
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}