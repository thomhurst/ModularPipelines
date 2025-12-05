using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs", "batch-start-viewer-session-revocation")]
public record AwsIvsBatchStartViewerSessionRevocationOptions(
[property: CliOption("--viewer-sessions")] string[] ViewerSessions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}