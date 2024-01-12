using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "findings", "list-marks")]
public record GcloudSccFindingsListMarksOptions(
[property: PositionalArgument] string Finding,
[property: PositionalArgument] string Organization,
[property: PositionalArgument] string Source
) : GcloudOptions
{
    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--read-time")]
    public string? ReadTime { get; set; }
}