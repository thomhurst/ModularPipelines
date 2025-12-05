using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "query-lineage")]
public record AwsSagemakerQueryLineageOptions : AwsOptions
{
    [CliOption("--start-arns")]
    public string[]? StartArns { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--max-depth")]
    public int? MaxDepth { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}