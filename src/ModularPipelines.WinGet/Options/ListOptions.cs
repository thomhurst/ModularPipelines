using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("list")]
public record ListOptions(
    [property: CommandSwitch("--query")] string Query
) : WingetOptions
{
    [CommandSwitch("--id")]
    public virtual string? Id { get; set; }

    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--moniker")]
    public virtual string? Moniker { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [BooleanCommandSwitch("--tag")]
    public virtual bool? Tag { get; set; }

    [BooleanCommandSwitch("--command")]
    public virtual bool? Command { get; set; }

    [BooleanCommandSwitch("--count")]
    public virtual bool? Count { get; set; }

    [BooleanCommandSwitch("--exact")]
    public virtual bool? Exact { get; set; }

    [CommandSwitch("--scope")]
    public virtual string? Scope { get; set; }

    [CommandSwitch("--header")]
    public virtual string? Header { get; set; }

    [CommandSwitch("--accept-source-agreements")]
    public virtual string? AcceptSourceAgreements { get; set; }

    [BooleanCommandSwitch("--upgrade-available")]
    public virtual bool? UpgradeAvailable { get; set; }

    [BooleanCommandSwitch("--wait")]
    public virtual bool? Wait { get; set; }

    [BooleanCommandSwitch("--open-logs")]
    public virtual bool? OpenLogs { get; set; }

    [BooleanCommandSwitch("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    [BooleanCommandSwitch("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; }
}