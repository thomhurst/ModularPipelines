using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("daemon")]
[ExcludeFromCodeCoverage]
public record GitDaemonOptions : GitOptions
{
    [BooleanCommandSwitch("--strict-paths")]
    public virtual bool? StrictPaths { get; set; }

    [CommandEqualsSeparatorSwitch("--base-path")]
    public string? BasePath { get; set; }

    [BooleanCommandSwitch("--base-path-relaxed")]
    public virtual bool? BasePathRelaxed { get; set; }

    [CommandEqualsSeparatorSwitch("--interpolated-path")]
    public string? InterpolatedPath { get; set; }

    [BooleanCommandSwitch("--export-all")]
    public virtual bool? ExportAll { get; set; }

    [BooleanCommandSwitch("--inetd")]
    public virtual bool? Inetd { get; set; }

    [CommandEqualsSeparatorSwitch("--listen")]
    public string? Listen { get; set; }

    [CommandEqualsSeparatorSwitch("--port")]
    public string? Port { get; set; }

    [CommandEqualsSeparatorSwitch("--init-timeout")]
    public string? InitTimeout { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandEqualsSeparatorSwitch("--max-connections")]
    public string? MaxConnections { get; set; }

    [BooleanCommandSwitch("--syslog")]
    public virtual bool? Syslog { get; set; }

    [CommandEqualsSeparatorSwitch("--log-destination")]
    public string? LogDestination { get; set; }

    [BooleanCommandSwitch("--user-path")]
    public virtual bool? UserPath { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--reuseaddr")]
    public virtual bool? Reuseaddr { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [CommandEqualsSeparatorSwitch("--pid-file")]
    public string? PidFile { get; set; }

    [CommandEqualsSeparatorSwitch("--user")]
    public string? User { get; set; }

    [CommandEqualsSeparatorSwitch("--group")]
    public string? Group { get; set; }

    [CommandEqualsSeparatorSwitch("--enable")]
    public string? Enable { get; set; }

    [CommandEqualsSeparatorSwitch("--disable")]
    public string? Disable { get; set; }

    [CommandEqualsSeparatorSwitch("--allow-override")]
    public string? AllowOverride { get; set; }

    [CommandEqualsSeparatorSwitch("--forbid-override")]
    public string? ForbidOverride { get; set; }

    [BooleanCommandSwitch("--no-informative-errors")]
    public virtual bool? NoInformativeErrors { get; set; }

    [BooleanCommandSwitch("--informative-errors")]
    public virtual bool? InformativeErrors { get; set; }

    [CommandEqualsSeparatorSwitch("--access-hook")]
    public string? AccessHook { get; set; }
}