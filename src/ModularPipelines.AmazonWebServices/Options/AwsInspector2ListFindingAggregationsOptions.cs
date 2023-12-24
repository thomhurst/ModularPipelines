using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector2", "list-finding-aggregations")]
public record AwsInspector2ListFindingAggregationsOptions(
[property: CommandSwitch("--aggregation-type")] string AggregationType
) : AwsOptions
{
    [CommandSwitch("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CommandSwitch("--aggregation-request")]
    public string? AggregationRequest { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}