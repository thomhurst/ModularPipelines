using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
public record ChocoOptions() : CommandLineToolOptions("choco")
{
    [CliFlag("--help")]
    public virtual bool? Help { get; set; }

    [CliFlag("--online")]
    public virtual bool? Online { get; set; }

    [CliFlag("--debug")]
    public virtual bool? Debug { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--trace")]
    public virtual bool? Trace { get; set; }

    [CliFlag("--no-color")]
    public virtual bool? NoColor { get; set; }

    [CliFlag("--accept-license")]
    public virtual bool? AcceptLicense { get; set; }

    [CliFlag("--confirm")]
    public virtual bool? Confirm { get; set; } = true;

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--what-if")]
    public virtual bool? WhatIf { get; set; }

    [CliFlag("--limit-output")]
    public virtual bool? LimitOutput { get; set; }

    [CliOption("--execution-timeout")]
    public virtual string? ExecutionTimeout { get; set; }

    [CliOption("--cache-location")]
    public virtual string? CacheLocation { get; set; }

    [CliFlag("--allow-unofficial-build")]
    public virtual bool? AllowUnofficialBuild { get; set; }

    [CliFlag("--fail-on-error-output")]
    public virtual bool? FailOnErrorOutput { get; set; }

    [CliFlag("--use-system-powershell")]
    public virtual bool? UseSystemPowershell { get; set; }

    [CliFlag("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [CliOption("--proxy")]
    public virtual string? Proxy { get; set; }

    [CliOption("--proxy-user")]
    public virtual string? ProxyUser { get; set; }

    [CliOption("--proxy-password")]
    public virtual string? ProxyPassword { get; set; }

    [CliOption("--proxy-bypass-list")]
    public virtual string? ProxyBypassList { get; set; }

    [CliFlag("--proxy-bypass-on-local")]
    public virtual bool? ProxyBypassOnLocal { get; set; }

    [CliOption("--log-file")]
    public virtual string? LogFile { get; set; }

    [CliFlag("--skip-compatibility-checks")]
    public virtual bool? SkipCompatibilityChecks { get; set; }

    [CliFlag("--ignore-http-cache")]
    public virtual bool? IgnoreHttpCache { get; set; }
}