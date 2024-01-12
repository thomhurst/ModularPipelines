using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "agent-pools", "list")]
public record GcloudTransferAgentPoolsListOptions : GcloudOptions
{
    [CommandSwitch("--limit")]
    public string? Limit { get; set; }

    [CommandSwitch("--names")]
    public string[]? Names { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }
}