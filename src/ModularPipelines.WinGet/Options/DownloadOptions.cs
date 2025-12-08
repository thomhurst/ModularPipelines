using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("download")]
public record DownloadOptions(
    [property: CliOption("--query")] string Query
) : WingetOptions
{
    [CliOption("--download-directory")]
    public virtual string? DownloadDirectory { get; set; }

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

    [CliOption("--scope")]
    public virtual string? Scope { get; set; }

    [CliOption("--architecture")]
    public virtual string? Architecture { get; set; }

    [CliOption("--installer-type")]
    public virtual string? InstallerType { get; set; }

    [CliFlag("--exact")]
    public virtual bool? Exact { get; set; }

    [CliOption("--locale")]
    public virtual string? Locale { get; set; }

    [CliOption("--log")]
    public virtual string? Log { get; set; }

    [CliFlag("--ignore-security-hash")]
    public virtual bool? IgnoreSecurityHash { get; set; }

    [CliFlag("--accept-package-agreements")]
    public virtual bool? AcceptPackageAgreements { get; set; }

    [CliOption("--accept-source-agreements")]
    public virtual string? AcceptSourceAgreements { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    [CliFlag("--open-logs")]
    public virtual bool? OpenLogs { get; set; }

    [CliFlag("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    [CliFlag("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; }
}