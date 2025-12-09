using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-explainability-export")]
public record AwsForecastCreateExplainabilityExportOptions(
[property: CliOption("--explainability-export-name")] string ExplainabilityExportName,
[property: CliOption("--explainability-arn")] string ExplainabilityArn,
[property: CliOption("--destination")] string Destination
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}