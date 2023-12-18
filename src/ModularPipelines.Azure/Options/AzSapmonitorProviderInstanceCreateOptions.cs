using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sapmonitor", "provider-instance", "create")]
public record AzSapmonitorProviderInstanceCreateOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--provider-instance-name")] string ProviderInstanceName,
[property: CommandSwitch("--provider-instance-properties")] string ProviderInstanceProperties,
[property: CommandSwitch("--provider-instance-type")] string ProviderInstanceType,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--provider-instance-metadata")]
    public string? ProviderInstanceMetadata { get; set; }
}