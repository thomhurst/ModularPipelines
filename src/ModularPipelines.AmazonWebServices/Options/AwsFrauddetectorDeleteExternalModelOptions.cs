using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "delete-external-model")]
public record AwsFrauddetectorDeleteExternalModelOptions(
[property: CliOption("--model-endpoint")] string ModelEndpoint
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}