using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("daemon")]
public record GitDaemonOptions : GitOptions
{
    [BooleanCommandSwitch("strict-paths")]
    public bool? StrictPaths { get; set; }

    [CommandLongSwitch("base-path")]
    public string? BasePath { get; set; }

    [BooleanCommandSwitch("base-path-relaxed")]
    public bool? BasePathRelaxed { get; set; }

    [CommandLongSwitch("interpolated-path")]
    public string? InterpolatedPath { get; set; }

    [BooleanCommandSwitch("export-all")]
    public bool? ExportAll { get; set; }

    [BooleanCommandSwitch("inetd")]
    public bool? Inetd { get; set; }

    [CommandLongSwitch("listen")]
    public string? Listen { get; set; }

    [CommandLongSwitch("port")]
    public string? Port { get; set; }

    [CommandLongSwitch("init-timeout")]
    public string? InitTimeout { get; set; }

    [CommandLongSwitch("timeout")]
    public string? Timeout { get; set; }

    [CommandLongSwitch("max-connections")]
    public string? MaxConnections { get; set; }

    [BooleanCommandSwitch("syslog")]
    public bool? Syslog { get; set; }

    [CommandLongSwitch("log-destination")]
    public string? LogDestination { get; set; }

    [BooleanCommandSwitch("user-path")]
    public bool? UserPath { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("reuseaddr")]
    public bool? Reuseaddr { get; set; }

    [BooleanCommandSwitch("detach")]
    public bool? Detach { get; set; }

    [CommandLongSwitch("pid-file")]
    public string? PidFile { get; set; }

    [CommandLongSwitch("user")]
    public string? User { get; set; }

    [CommandLongSwitch("group")]
    public string? Group { get; set; }

    [CommandLongSwitch("enable")]
    public string? Enable { get; set; }

    [CommandLongSwitch("disable")]
    public string? Disable { get; set; }

    [CommandLongSwitch("allow-override")]
    public string? AllowOverride { get; set; }

    [CommandLongSwitch("forbid-override")]
    public string? ForbidOverride { get; set; }

    [BooleanCommandSwitch("no-informative-errors")]
    public bool? NoInformativeErrors { get; set; }

    [BooleanCommandSwitch("informative-errors")]
    public bool? InformativeErrors { get; set; }

    [CommandLongSwitch("access-hook")]
    public string? AccessHook { get; set; }

}