using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("xray", "get-sampling-targets")]
public record AwsXrayGetSamplingTargetsOptions(
[property: CliOption("--sampling-statistics-documents")] string[] SamplingStatisticsDocuments
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}