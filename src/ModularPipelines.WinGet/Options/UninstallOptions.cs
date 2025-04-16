using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("uninstall")]
public record UninstallOptions(
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

    [BooleanCommandSwitch("--product-code")]
    public virtual bool? ProductCode { get; set; }

    [BooleanCommandSwitch("--version")]
    public virtual bool? Version { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [BooleanCommandSwitch("--exact")]
    public virtual bool? Exact { get; set; }

    [CommandSwitch("--scope")]
    public virtual string? Scope { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--silent")]
    public virtual bool? Silent { get; set; } = true;

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--purge")]
    public virtual bool? Purge { get; set; }

    [BooleanCommandSwitch("--preserve")]
    public virtual bool? Preserve { get; set; }

    [CommandSwitch("--log")]
    public virtual string? Log { get; set; }

    [CommandSwitch("--header")]
    public virtual string? Header { get; set; }

    [CommandSwitch("--accept-source-agreements")]
    public virtual string? AcceptSourceAgreements { get; set; }

    [BooleanCommandSwitch("--wait")]
    public virtual bool? Wait { get; set; }

    [BooleanCommandSwitch("--open-logs")]
    public virtual bool? OpenLogs { get; set; }

    [BooleanCommandSwitch("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    [BooleanCommandSwitch("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; }
}