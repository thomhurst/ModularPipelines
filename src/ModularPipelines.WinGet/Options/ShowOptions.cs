using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("show")]
public record ShowOptions(
    [property: CommandSwitch("--query")] string Query
) : WingetOptions
{
    [CommandSwitch("--manifest")]
    public string? Manifest { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--moniker")]
    public string? Moniker { get; set; }

    [BooleanCommandSwitch("--version")]
    public bool? Version { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [BooleanCommandSwitch("--exact")]
    public bool? Exact { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--installer-type")]
    public string? InstallerType { get; set; }

    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [BooleanCommandSwitch("--versions")]
    public bool? Versions { get; set; }

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