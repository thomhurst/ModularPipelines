using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "trigger-run", "query-by-factory")]
public record AzDatafactoryTriggerRunQueryByFactoryOptions(
[property: CliOption("--last-updated-after")] string LastUpdatedAfter,
[property: CliOption("--last-updated-before")] string LastUpdatedBefore
) : AzOptions
{
    [CliOption("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CliOption("--factory-name")]
    public string? FactoryName { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}