using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "node", "user", "delete")]
public record AzBatchNodeUserDeleteOptions(
[property: CliOption("--node-id")] string NodeId,
[property: CliOption("--pool-id")] string PoolId,
[property: CliOption("--user-name")] string UserName
) : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}