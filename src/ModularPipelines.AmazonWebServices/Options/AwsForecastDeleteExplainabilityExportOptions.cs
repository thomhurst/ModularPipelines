using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "delete-explainability-export")]
public record AwsForecastDeleteExplainabilityExportOptions(
[property: CliOption("--explainability-export-arn")] string ExplainabilityExportArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}