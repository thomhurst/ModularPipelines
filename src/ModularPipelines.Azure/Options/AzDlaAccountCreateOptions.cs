using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "account", "create")]
public record AzDlaAccountCreateOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--default-data-lake-store")] string DefaultDataLakeStore
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--max-degree-of-parallelism")]
    public string? MaxDegreeOfParallelism { get; set; }

    [CliOption("--max-job-count")]
    public int? MaxJobCount { get; set; }

    [CliOption("--query-store-retention")]
    public string? QueryStoreRetention { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }
}