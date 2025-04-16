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
    public virtual string? Manifest { get; set; }

    [CommandSwitch("--id")]
    public virtual string? Id { get; set; }

    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--moniker")]
    public virtual string? Moniker { get; set; }

    [BooleanCommandSwitch("--version")]
    public virtual bool? Version { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [BooleanCommandSwitch("--exact")]
    public virtual bool? Exact { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--silent")]
    public virtual bool? Silent { get; set; } = true;

    [BooleanCommandSwitch("--purge")]
    public virtual bool? Purge { get; set; }

    [CommandSwitch("--log")]
    public virtual string? Log { get; set; }

    [CommandSwitch("--custom")]
    public virtual string[]? Custom { get; set; }

    [CommandSwitch("--override")]
    public virtual string? Override { get; set; }

    [CommandSwitch("--location")]
    public virtual string? Location { get; set; }

    [CommandSwitch("-scope")]
    public virtual string? Scope { get; set; }

    [CommandSwitch("--architecture")]
    public virtual string? Architecture { get; set; }

    [CommandSwitch("--installer-type")]
    public virtual string? InstallerType { get; set; }

    [CommandSwitch("--locale")]
    public virtual string? Locale { get; set; }

    [BooleanCommandSwitch("--ignore-security-hash")]
    public virtual bool? IgnoreSecurityHash { get; set; }

    [BooleanCommandSwitch("--ignore-local-archive-malware-scan")]
    public virtual bool? IgnoreLocalArchiveMalwareScan { get; set; }

    [BooleanCommandSwitch("--accept-package-agreements")]
    public virtual bool? AcceptPackageAgreements { get; set; }

    [CommandSwitch("--accept-source-agreements")]
    public virtual string? AcceptSourceAgreements { get; set; }

    [CommandSwitch("--header")]
    public virtual string? Header { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--include-unknown")]
    public virtual bool? IncludeUnknown { get; set; }

    [BooleanCommandSwitch("--include-pinned")]
    public virtual bool? IncludePinned { get; set; }

    [BooleanCommandSwitch("--uninstall-previous")]
    public virtual bool? UninstallPrevious { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--wait")]
    public virtual bool? Wait { get; set; }

    [BooleanCommandSwitch("--open-logs")]
    public virtual bool? OpenLogs { get; set; }

    [BooleanCommandSwitch("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    [BooleanCommandSwitch("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; }
}