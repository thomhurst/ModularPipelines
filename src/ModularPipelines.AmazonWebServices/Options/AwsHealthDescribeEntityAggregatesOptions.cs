using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("health", "describe-entity-aggregates")]
public record AwsHealthDescribeEntityAggregatesOptions : AwsOptions
{
    [CliOption("--event-arns")]
    public string[]? EventArns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}