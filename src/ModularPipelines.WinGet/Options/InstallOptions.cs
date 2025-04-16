using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("install")]
public record InstallOptions(
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

    [CommandSwitch("--scope")]
    public virtual string? Scope { get; set; }

    [CommandSwitch("--architecture")]
    public virtual string? Architecture { get; set; }

    [CommandSwitch("--installer-type")]
    public virtual string? InstallerType { get; set; }

    [BooleanCommandSwitch("--exact")]
    public virtual bool? Exact { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--silent")]
    public virtual bool? Silent { get; set; } = true;

    [CommandSwitch("--locale")]
    public virtual string? Locale { get; set; }

    [CommandSwitch("--log")]
    public virtual string? Log { get; set; }

    [CommandSwitch("--custom")]
    public virtual string[]? Custom { get; set; }

    [CommandSwitch("--override")]
    public virtual string? Override { get; set; }

    [CommandSwitch("--location")]
    public virtual string? Location { get; set; }

    [BooleanCommandSwitch("--ignore-security-hash")]
    public virtual bool? IgnoreSecurityHash { get; set; }

    [BooleanCommandSwitch("--ignore-local-archive-malware-scan")]
    public virtual bool? IgnoreLocalArchiveMalwareScan { get; set; }

    [CommandSwitch("--dependency-source")]
    public virtual string? DependencySource { get; set; }

    [BooleanCommandSwitch("--accept-package-agreements")]
    public virtual bool? AcceptPackageAgreements { get; set; }

    [CommandSwitch("--accept-source-agreements")]
    public virtual string? AcceptSourceAgreements { get; set; }

    [BooleanCommandSwitch("--no-upgrade")]
    public virtual bool? NoUpgrade { get; set; }

    [CommandSwitch("--header")]
    public virtual string? Header { get; set; }

    [CommandSwitch("--rename")]
    public virtual string? Rename { get; set; }

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