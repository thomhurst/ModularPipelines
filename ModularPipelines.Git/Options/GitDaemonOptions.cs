using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("daemon")]
public record GitDaemonOptions : GitOptions
{
    [BooleanCommandSwitch("--strict-paths")]
    public bool? StrictPaths { get; set; }

    [CommandEqualsSeparatorSwitch("--base-path")]
    public string? BasePath { get; set; }

    [BooleanCommandSwitch("--base-path-relaxed")]
    public bool? BasePathRelaxed { get; set; }

    [CommandEqualsSeparatorSwitch("--interpolated-path")]
    public string? InterpolatedPath { get; set; }

    [BooleanCommandSwitch("--export-all")]
    public bool? ExportAll { get; set; }

    [BooleanCommandSwitch("--inetd")]
    public bool? Inetd { get; set; }

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
    public bool? Syslog { get; set; }

    [CommandEqualsSeparatorSwitch("--log-destination")]
    public string? LogDestination { get; set; }

    [BooleanCommandSwitch("--user-path")]
    public bool? UserPath { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--reuseaddr")]
    public bool? Reuseaddr { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

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
    public bool? NoInformativeErrors { get; set; }

    [BooleanCommandSwitch("--informative-errors")]
    public bool? InformativeErrors { get; set; }

    [CommandEqualsSeparatorSwitch("--access-hook")]
    public string? AccessHook { get; set; }
}