using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "get-aggregate-conformance-pack-compliance-summary")]
public record AwsConfigserviceGetAggregateConformancePackComplianceSummaryOptions(
[property: CommandSwitch("--configuration-aggregator-name")] string ConfigurationAggregatorName
) : AwsOptions
{
    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--group-by-key")]
    public string? GroupByKey { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}