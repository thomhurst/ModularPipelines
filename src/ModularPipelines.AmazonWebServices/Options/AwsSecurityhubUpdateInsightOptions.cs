using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "update-insight")]
public record AwsSecurityhubUpdateInsightOptions(
[property: CliOption("--insight-arn")] string InsightArn
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--group-by-attribute")]
    public string? GroupByAttribute { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}