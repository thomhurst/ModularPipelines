using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-endpoint-weights-and-capacities")]
public record AwsSagemakerUpdateEndpointWeightsAndCapacitiesOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--desired-weights-and-capacities")] string[] DesiredWeightsAndCapacities
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}