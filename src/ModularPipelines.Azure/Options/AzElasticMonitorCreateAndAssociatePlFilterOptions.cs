using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic", "monitor", "create-and-associate-pl-filter")]
public record AzElasticMonitorCreateAndAssociatePlFilterOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--private-endpoint-guid")]
    public string? PrivateEndpointGuid { get; set; }

    [CommandSwitch("--private-endpoint-name")]
    public string? PrivateEndpointName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

