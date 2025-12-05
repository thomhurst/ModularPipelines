using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "get-models")]
public record AwsFrauddetectorGetModelsOptions : AwsOptions
{
    [CliOption("--model-id")]
    public string? ModelId { get; set; }

    [CliOption("--model-type")]
    public string? ModelType { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}