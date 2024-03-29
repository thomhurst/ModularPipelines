using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("upgrade")]
public record UpgradeOptions(
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

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--silent")]
    public bool? Silent { get; set; } = true;

    [BooleanCommandSwitch("--purge")]
    public bool? Purge { get; set; }

    [CommandSwitch("--log")]
    public string? Log { get; set; }

    [CommandSwitch("--custom")]
    public string[]? Custom { get; set; }

    [CommandSwitch("--override")]
    public string? Override { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("-scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--installer-type")]
    public string? InstallerType { get; set; }

    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [BooleanCommandSwitch("--ignore-security-hash")]
    public bool? IgnoreSecurityHash { get; set; }

    [BooleanCommandSwitch("--ignore-local-archive-malware-scan")]
    public bool? IgnoreLocalArchiveMalwareScan { get; set; }

    [BooleanCommandSwitch("--accept-package-agreements")]
    public bool? AcceptPackageAgreements { get; set; }

    [CommandSwitch("--accept-source-agreements")]
    public string? AcceptSourceAgreements { get; set; }

    [CommandSwitch("--header")]
    public string? Header { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--include-unknown")]
    public bool? IncludeUnknown { get; set; }

    [BooleanCommandSwitch("--include-pinned")]
    public bool? IncludePinned { get; set; }

    [BooleanCommandSwitch("--uninstall-previous")]
    public bool? UninstallPrevious { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }

    [BooleanCommandSwitch("--open-logs")]
    public bool? OpenLogs { get; set; }

    [BooleanCommandSwitch("--verbose-logs")]
    public bool? VerboseLogs { get; set; }

    [BooleanCommandSwitch("--disable-interactivity")]
    public bool? DisableInteractivity { get; set; }
}