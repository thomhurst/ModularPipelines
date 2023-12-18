using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "account", "create")]
public record AzDlaAccountCreateOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--default-data-lake-store")] string DefaultDataLakeStore
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--max-degree-of-parallelism")]
    public string? MaxDegreeOfParallelism { get; set; }

    [CommandSwitch("--max-job-count")]
    public int? MaxJobCount { get; set; }

    [CommandSwitch("--query-store-retention")]
    public string? QueryStoreRetention { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }
}