using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "get-model-version")]
public record AwsFrauddetectorGetModelVersionOptions(
[property: CliOption("--model-id")] string ModelId,
[property: CliOption("--model-type")] string ModelType,
[property: CliOption("--model-version-number")] string ModelVersionNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}