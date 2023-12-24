using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "get-cardinality")]
public record AwsIotGetCardinalityOptions(
[property: CommandSwitch("--query-string")] string QueryString
) : AwsOptions
{
    [CommandSwitch("--index-name")]
    public string? IndexName { get; set; }

    [CommandSwitch("--aggregation-field")]
    public string? AggregationField { get; set; }

    [CommandSwitch("--query-version")]
    public string? QueryVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}