using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "pool", "list")]
public record AzBatchPoolListOptions : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--select")]
    public string? Select { get; set; }
}