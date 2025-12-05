using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "delete-ml-model")]
public record AwsMachinelearningDeleteMlModelOptions(
[property: CliOption("--ml-model-id")] string MlModelId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}