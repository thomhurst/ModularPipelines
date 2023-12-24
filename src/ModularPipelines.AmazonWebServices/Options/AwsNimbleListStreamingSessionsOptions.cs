using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "list-streaming-sessions")]
public record AwsNimbleListStreamingSessionsOptions(
[property: CommandSwitch("--studio-id")] string StudioId
) : AwsOptions
{
    [CommandSwitch("--created-by")]
    public string? CreatedBy { get; set; }

    [CommandSwitch("--owned-by")]
    public string? OwnedBy { get; set; }

    [CommandSwitch("--session-ids")]
    public string? SessionIds { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}