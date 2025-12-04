using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "incident", "comment", "create")]
public record AzSentinelIncidentCommentCreateOptions(
[property: CliOption("--incident-comment-id")] string IncidentCommentId,
[property: CliOption("--incident-id")] string IncidentId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--message")]
    public string? Message { get; set; }
}