using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "trigger", "query-by-factory")]
public record AzDatafactoryTriggerQueryByFactoryOptions : AzOptions
{
    [CliOption("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CliOption("--factory-name")]
    public string? FactoryName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--parent-trigger-name")]
    public string? ParentTriggerName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}