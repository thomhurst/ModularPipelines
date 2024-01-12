using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "findings", "group")]
public record GcloudSccFindingsGroupOptions(
[property: PositionalArgument] string Parent
) : GcloudOptions
{
    [CommandSwitch("--compare-duration")]
    public string? CompareDuration { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--group-by")]
    public string? GroupBy { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--read-time")]
    public string? ReadTime { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }
}