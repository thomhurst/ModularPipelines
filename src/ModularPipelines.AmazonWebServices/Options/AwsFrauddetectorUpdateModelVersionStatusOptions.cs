using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "update-model-version-status")]
public record AwsFrauddetectorUpdateModelVersionStatusOptions(
[property: CliOption("--model-id")] string ModelId,
[property: CliOption("--model-type")] string ModelType,
[property: CliOption("--model-version-number")] string ModelVersionNumber,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}