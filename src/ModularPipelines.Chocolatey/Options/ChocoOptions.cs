using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
public record ChocoOptions() : CommandLineToolOptions("choco")
{
    [BooleanCommandSwitch("--help")]
    public bool? Help { get; set; }

    [BooleanCommandSwitch("--online")]
    public bool? Online { get; set; }

    [BooleanCommandSwitch("--debug")]
    public bool? Debug { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--trace")]
    public bool? Trace { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public bool? NoColor { get; set; }

    [BooleanCommandSwitch("--accept-license")]
    public bool? AcceptLicense { get; set; }

    [BooleanCommandSwitch("--confirm")]
    public bool? Confirm { get; set; } = true;

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--what-if")]
    public bool? WhatIf { get; set; }

    [BooleanCommandSwitch("--limit-output")]
    public bool? LimitOutput { get; set; }

    [CommandSwitch("--execution-timeout")]
    public string? ExecutionTimeout { get; set; }

    [CommandSwitch("--cache-location")]
    public string? CacheLocation { get; set; }

    [BooleanCommandSwitch("--allow-unofficial-build")]
    public bool? AllowUnofficialBuild { get; set; }

    [BooleanCommandSwitch("--fail-on-error-output")]
    public bool? FailOnErrorOutput { get; set; }

    [BooleanCommandSwitch("--use-system-powershell")]
    public bool? UseSystemPowershell { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public bool? NoProgress { get; set; }

    [CommandSwitch("--proxy")]
    public string? Proxy { get; set; }

    [CommandSwitch("--proxy-user")]
    public string? ProxyUser { get; set; }

    [CommandSwitch("--proxy-password")]
    public string? ProxyPassword { get; set; }

    [CommandSwitch("--proxy-bypass-list")]
    public string? ProxyBypassList { get; set; }

    [BooleanCommandSwitch("--proxy-bypass-on-local")]
    public bool? ProxyBypassOnLocal { get; set; }

    [CommandSwitch("--log-file")]
    public string? LogFile { get; set; }

    [BooleanCommandSwitch("--skip-compatibility-checks")]
    public bool? SkipCompatibilityChecks { get; set; }

    [BooleanCommandSwitch("--ignore-http-cache")]
    public bool? IgnoreHttpCache { get; set; }
}