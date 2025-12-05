using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "list-ml-model-training-jobs")]
public record AwsNeptunedataListMlModelTrainingJobsOptions : AwsOptions
{
    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--neptune-iam-role-arn")]
    public string? NeptuneIamRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}