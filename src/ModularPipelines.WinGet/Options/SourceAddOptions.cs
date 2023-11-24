using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("source", "add")]
public record SourceAddOptions(
    [property: CommandSwitch("--name")] string Name,
    [property: BooleanCommandSwitch("--arg")] bool Arg,
    [property: CommandSwitch("--type")] string Type
) : WingetOptions
{
    [CommandSwitch("--header")]
    public string? Header { get; set; }

    [CommandSwitch("--accept-source-agreements")]
    public string? AcceptSourceAgreements { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }

    [BooleanCommandSwitch("--open-logs")]
    public bool? OpenLogs { get; set; }

    [BooleanCommandSwitch("--verbose-logs")]
    public bool? VerboseLogs { get; set; }

    [BooleanCommandSwitch("--disable-interactivity")]
    public bool? DisableInteractivity { get; set; }
}