using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "findings", "set-mute")]
public record GcloudSccFindingsSetMuteOptions(
[property: PositionalArgument] string Finding,
[property: CommandSwitch("--mute")] string Mute
) : GcloudOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}