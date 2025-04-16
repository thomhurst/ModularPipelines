using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
public record ChocoOptions() : CommandLineToolOptions("choco")
{
    [BooleanCommandSwitch("--help")]
    public virtual bool? Help { get; set; }

    [BooleanCommandSwitch("--online")]
    public virtual bool? Online { get; set; }

    [BooleanCommandSwitch("--debug")]
    public virtual bool? Debug { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--trace")]
    public virtual bool? Trace { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public virtual bool? NoColor { get; set; }

    [BooleanCommandSwitch("--accept-license")]
    public virtual bool? AcceptLicense { get; set; }

    [BooleanCommandSwitch("--confirm")]
    public virtual bool? Confirm { get; set; } = true;

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--what-if")]
    public virtual bool? WhatIf { get; set; }

    [BooleanCommandSwitch("--limit-output")]
    public virtual bool? LimitOutput { get; set; }

    [CommandSwitch("--execution-timeout")]
    public virtual string? ExecutionTimeout { get; set; }

    [CommandSwitch("--cache-location")]
    public virtual string? CacheLocation { get; set; }

    [BooleanCommandSwitch("--allow-unofficial-build")]
    public virtual bool? AllowUnofficialBuild { get; set; }

    [BooleanCommandSwitch("--fail-on-error-output")]
    public virtual bool? FailOnErrorOutput { get; set; }

    [BooleanCommandSwitch("--use-system-powershell")]
    public virtual bool? UseSystemPowershell { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [CommandSwitch("--proxy")]
    public virtual string? Proxy { get; set; }

    [CommandSwitch("--proxy-user")]
    public virtual string? ProxyUser { get; set; }

    [CommandSwitch("--proxy-password")]
    public virtual string? ProxyPassword { get; set; }

    [CommandSwitch("--proxy-bypass-list")]
    public virtual string? ProxyBypassList { get; set; }

    [BooleanCommandSwitch("--proxy-bypass-on-local")]
    public virtual bool? ProxyBypassOnLocal { get; set; }

    [CommandSwitch("--log-file")]
    public virtual string? LogFile { get; set; }

    [BooleanCommandSwitch("--skip-compatibility-checks")]
    public virtual bool? SkipCompatibilityChecks { get; set; }

    [BooleanCommandSwitch("--ignore-http-cache")]
    public virtual bool? IgnoreHttpCache { get; set; }
}