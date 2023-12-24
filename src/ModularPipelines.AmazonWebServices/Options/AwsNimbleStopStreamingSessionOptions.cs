using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "stop-streaming-session")]
public record AwsNimbleStopStreamingSessionOptions(
[property: CommandSwitch("--session-id")] string SessionId,
[property: CommandSwitch("--studio-id")] string StudioId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--volume-retention-mode")]
    public string? VolumeRetentionMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}