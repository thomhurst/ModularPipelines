using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sapmonitor", "provider-instance", "create")]
public record AzSapmonitorProviderInstanceCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--provider-instance-name")] string ProviderInstanceName,
[property: CliOption("--provider-instance-properties")] string ProviderInstanceProperties,
[property: CliOption("--provider-instance-type")] string ProviderInstanceType,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--provider-instance-metadata")]
    public string? ProviderInstanceMetadata { get; set; }
}