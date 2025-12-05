using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "support", "create-bundle")]
public record AzIotOpsSupportCreateBundleOptions : AzOptions
{
    [CliOption("--bundle-dir")]
    public string? BundleDir { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--log-age")]
    public string? LogAge { get; set; }

    [CliFlag("--mq-traces")]
    public bool? MqTraces { get; set; }

    [CliOption("--ops-service")]
    public string? OpsService { get; set; }
}