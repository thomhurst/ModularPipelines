using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "node", "user", "delete")]
public record AzBatchNodeUserDeleteOptions(
[property: CommandSwitch("--node-id")] string NodeId,
[property: CommandSwitch("--pool-id")] string PoolId,
[property: CommandSwitch("--user-name")] string UserName
) : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}