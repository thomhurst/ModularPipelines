using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sapmonitor", "provider-instance", "delete")]
public record AzSapmonitorProviderInstanceDeleteOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--provider-instance-name")] string ProviderInstanceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;