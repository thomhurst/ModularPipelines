using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "incident", "comment", "show")]
public record AzSentinelIncidentCommentShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--incident-comment-id")]
    public string? IncidentCommentId { get; set; }

    [CliOption("--incident-id")]
    public string? IncidentId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}