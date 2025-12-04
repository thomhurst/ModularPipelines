using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "node", "file", "delete")]
public record AzBatchNodeFileDeleteOptions(
[property: CliOption("--file-path")] string FilePath,
[property: CliOption("--node-id")] string NodeId,
[property: CliOption("--pool-id")] string PoolId
) : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--recursive")]
    public string? Recursive { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}