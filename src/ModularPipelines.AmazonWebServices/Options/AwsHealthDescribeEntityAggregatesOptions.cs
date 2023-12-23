using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("health", "describe-entity-aggregates")]
public record AwsHealthDescribeEntityAggregatesOptions : AwsOptions
{
    [CommandSwitch("--event-arns")]
    public string[]? EventArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}