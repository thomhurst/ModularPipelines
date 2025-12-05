using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "node", "file", "show")]
public record AzBatchNodeFileShowOptions(
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

    [CliOption("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CliOption("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }
}