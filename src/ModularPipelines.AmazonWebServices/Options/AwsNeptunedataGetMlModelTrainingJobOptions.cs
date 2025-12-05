using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "get-ml-model-training-job")]
public record AwsNeptunedataGetMlModelTrainingJobOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}