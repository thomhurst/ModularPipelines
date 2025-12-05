using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "describe-explainability")]
public record AwsForecastDescribeExplainabilityOptions(
[property: CliOption("--explainability-arn")] string ExplainabilityArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}