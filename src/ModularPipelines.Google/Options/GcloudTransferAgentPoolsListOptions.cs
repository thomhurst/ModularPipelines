using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "agent-pools", "list")]
public record GcloudTransferAgentPoolsListOptions : GcloudOptions
{
    [CliOption("--limit")]
    public string? Limit { get; set; }

    [CliOption("--names")]
    public string[]? Names { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }
}