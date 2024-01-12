using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "findings", "list")]
public record GcloudSccFindingsListOptions(
[property: PositionalArgument] string Parent
) : GcloudOptions
{
    [CommandSwitch("--compare-duration")]
    public string? CompareDuration { get; set; }

    [CommandSwitch("--field-mask")]
    public string? FieldMask { get; set; }

    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }

    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--read-time")]
    public string? ReadTime { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }
}