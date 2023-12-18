using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "node", "scheduling", "disable")]
public record AzBatchNodeSchedulingDisableOptions(
[property: CommandSwitch("--node-id")] string NodeId,
[property: CommandSwitch("--pool-id")] string PoolId
) : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--node-disable-scheduling-option")]
    public bool? NodeDisableSchedulingOption { get; set; }
}