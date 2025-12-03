using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("daemon")]
[ExcludeFromCodeCoverage]
public record GitDaemonOptions : GitOptions
{
    [CliFlag("--strict-paths")]
    public virtual bool? StrictPaths { get; set; }

    [CliOption("--base-path", Format = OptionFormat.EqualsSeparated)]
    public string? BasePath { get; set; }

    [CliFlag("--base-path-relaxed")]
    public virtual bool? BasePathRelaxed { get; set; }

    [CliOption("--interpolated-path", Format = OptionFormat.EqualsSeparated)]
    public string? InterpolatedPath { get; set; }

    [CliFlag("--export-all")]
    public virtual bool? ExportAll { get; set; }

    [CliFlag("--inetd")]
    public virtual bool? Inetd { get; set; }

    [CliOption("--listen", Format = OptionFormat.EqualsSeparated)]
    public string? Listen { get; set; }

    [CliOption("--port", Format = OptionFormat.EqualsSeparated)]
    public string? Port { get; set; }

    [CliOption("--init-timeout", Format = OptionFormat.EqualsSeparated)]
    public string? InitTimeout { get; set; }

    [CliOption("--timeout", Format = OptionFormat.EqualsSeparated)]
    public string? Timeout { get; set; }

    [CliOption("--max-connections", Format = OptionFormat.EqualsSeparated)]
    public string? MaxConnections { get; set; }

    [CliFlag("--syslog")]
    public virtual bool? Syslog { get; set; }

    [CliOption("--log-destination", Format = OptionFormat.EqualsSeparated)]
    public string? LogDestination { get; set; }

    [CliFlag("--user-path")]
    public virtual bool? UserPath { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--reuseaddr")]
    public virtual bool? Reuseaddr { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliOption("--pid-file", Format = OptionFormat.EqualsSeparated)]
    public string? PidFile { get; set; }

    [CliOption("--user", Format = OptionFormat.EqualsSeparated)]
    public string? User { get; set; }

    [CliOption("--group", Format = OptionFormat.EqualsSeparated)]
    public string? Group { get; set; }

    [CliOption("--enable", Format = OptionFormat.EqualsSeparated)]
    public string? Enable { get; set; }

    [CliOption("--disable", Format = OptionFormat.EqualsSeparated)]
    public string? Disable { get; set; }

    [CliOption("--allow-override", Format = OptionFormat.EqualsSeparated)]
    public string? AllowOverride { get; set; }

    [CliOption("--forbid-override", Format = OptionFormat.EqualsSeparated)]
    public string? ForbidOverride { get; set; }

    [CliFlag("--no-informative-errors")]
    public virtual bool? NoInformativeErrors { get; set; }

    [CliFlag("--informative-errors")]
    public virtual bool? InformativeErrors { get; set; }

    [CliOption("--access-hook", Format = OptionFormat.EqualsSeparated)]
    public string? AccessHook { get; set; }
}