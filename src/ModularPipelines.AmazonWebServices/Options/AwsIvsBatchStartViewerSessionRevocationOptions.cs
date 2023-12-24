using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivs", "batch-start-viewer-session-revocation")]
public record AwsIvsBatchStartViewerSessionRevocationOptions(
[property: CommandSwitch("--viewer-sessions")] string[] ViewerSessions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}