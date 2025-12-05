using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CliCommand("uninstall")]
public record UninstallOptions(
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

    [CliFlag("--product-code")]
    public virtual bool? ProductCode { get; set; }

    [CliFlag("--version")]
    public virtual bool? Version { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliFlag("--exact")]
    public virtual bool? Exact { get; set; }

    [CliOption("--scope")]
    public virtual string? Scope { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--silent")]
    public virtual bool? Silent { get; set; } = true;

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--purge")]
    public virtual bool? Purge { get; set; }

    [CliFlag("--preserve")]
    public virtual bool? Preserve { get; set; }

    [CliOption("--log")]
    public virtual string? Log { get; set; }

    [CliOption("--header")]
    public virtual string? Header { get; set; }

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