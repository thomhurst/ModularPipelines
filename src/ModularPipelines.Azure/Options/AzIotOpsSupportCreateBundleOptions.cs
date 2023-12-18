using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "support", "create-bundle")]
public record AzIotOpsSupportCreateBundleOptions : AzOptions
{
    [CommandSwitch("--bundle-dir")]
    public string? BundleDir { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--log-age")]
    public string? LogAge { get; set; }

    [BooleanCommandSwitch("--mq-traces")]
    public bool? MqTraces { get; set; }

    [CommandSwitch("--ops-service")]
    public string? OpsService { get; set; }
}