using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("list")]
public record ListOptions(
    [property: CliOption("--query")] string Query
) : WingetOptions
{
    [CliOption("--id")]
    public virtual string? Id { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--moniker")]
    public virtual string? Moniker { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliFlag("--tag")]
    public virtual bool? Tag { get; set; }

    [CliFlag("--command")]
    public virtual bool? Command { get; set; }

    [CliFlag("--count")]
    public virtual bool? Count { get; set; }

    [CliFlag("--exact")]
    public virtual bool? Exact { get; set; }

    [CliOption("--scope")]
    public virtual string? Scope { get; set; }

    [CliOption("--header")]
    public virtual string? Header { get; set; }

    [CliOption("--accept-source-agreements")]
    public virtual string? AcceptSourceAgreements { get; set; }

    [CliFlag("--upgrade-available")]
    public virtual bool? UpgradeAvailable { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    [CliFlag("--open-logs")]
    public virtual bool? OpenLogs { get; set; }

    [CliFlag("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    [CliFlag("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; }
}