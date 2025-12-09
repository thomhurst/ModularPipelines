using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "delete-model-version")]
public record AwsFrauddetectorDeleteModelVersionOptions(
[property: CliOption("--model-id")] string ModelId,
[property: CliOption("--model-type")] string ModelType,
[property: CliOption("--model-version-number")] string ModelVersionNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}