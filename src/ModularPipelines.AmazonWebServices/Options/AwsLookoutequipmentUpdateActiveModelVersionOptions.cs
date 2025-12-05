using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "update-active-model-version")]
public record AwsLookoutequipmentUpdateActiveModelVersionOptions(
[property: CliOption("--model-name")] string ModelName,
[property: CliOption("--model-version")] long ModelVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}