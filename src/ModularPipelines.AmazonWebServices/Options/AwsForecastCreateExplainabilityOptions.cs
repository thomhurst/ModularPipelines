using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-explainability")]
public record AwsForecastCreateExplainabilityOptions(
[property: CliOption("--explainability-name")] string ExplainabilityName,
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--explainability-config")] string ExplainabilityConfig
) : AwsOptions
{
    [CliOption("--data-source")]
    public string? DataSource { get; set; }

    [CliOption("--schema")]
    public string? Schema { get; set; }

    [CliOption("--start-date-time")]
    public string? StartDateTime { get; set; }

    [CliOption("--end-date-time")]
    public string? EndDateTime { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}