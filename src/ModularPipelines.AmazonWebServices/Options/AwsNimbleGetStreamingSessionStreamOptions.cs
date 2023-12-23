using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "get-streaming-session-stream")]
public record AwsNimbleGetStreamingSessionStreamOptions(
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--stream-id")] string StreamId,
[property: CommandSwitch("--studio-id")] string StudioId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}