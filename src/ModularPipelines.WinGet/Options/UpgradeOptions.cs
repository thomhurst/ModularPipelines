using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("upgrade")]
public record UpgradeOptions(
    [property: CliOption("--query")] string Query
) : WingetOptions
{
    [CliOption("--manifest")]
    public virtual string? Manifest { get; set; }

    [CliOption("--id")]
    public virtual string? Id { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--moniker")]
    public virtual string? Moniker { get; set; }

    [CliFlag("--version")]
    public virtual bool? Version { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliFlag("--exact")]
    public virtual bool? Exact { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--silent")]
    public virtual bool? Silent { get; set; } = true;

    [CliFlag("--purge")]
    public virtual bool? Purge { get; set; }

    [CliOption("--log")]
    public virtual string? Log { get; set; }

    [CliOption("--custom")]
    public virtual string[]? Custom { get; set; }

    [CliOption("--override")]
    public virtual string? Override { get; set; }

    [CliOption("--location")]
    public virtual string? Location { get; set; }

    [CliOption("-scope")]
    public virtual string? Scope { get; set; }

    [CliOption("--architecture")]
    public virtual string? Architecture { get; set; }

    [CliOption("--installer-type")]
    public virtual string? InstallerType { get; set; }

    [CliOption("--locale")]
    public virtual string? Locale { get; set; }

    [CliFlag("--ignore-security-hash")]
    public virtual bool? IgnoreSecurityHash { get; set; }

    [CliFlag("--ignore-local-archive-malware-scan")]
    public virtual bool? IgnoreLocalArchiveMalwareScan { get; set; }

    [CliFlag("--accept-package-agreements")]
    public virtual bool? AcceptPackageAgreements { get; set; }

    [CliOption("--accept-source-agreements")]
    public virtual string? AcceptSourceAgreements { get; set; }

    [CliOption("--header")]
    public virtual string? Header { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--include-unknown")]
    public virtual bool? IncludeUnknown { get; set; }

    [CliFlag("--include-pinned")]
    public virtual bool? IncludePinned { get; set; }

    [CliFlag("--uninstall-previous")]
    public virtual bool? UninstallPrevious { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    [CliFlag("--open-logs")]
    public virtual bool? OpenLogs { get; set; }

    [CliFlag("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    [CliFlag("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; }
}