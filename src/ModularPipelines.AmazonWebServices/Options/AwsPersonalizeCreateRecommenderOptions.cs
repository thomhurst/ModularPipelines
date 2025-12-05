using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-recommender")]
public record AwsPersonalizeCreateRecommenderOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--dataset-group-arn")] string DatasetGroupArn,
[property: CliOption("--recipe-arn")] string RecipeArn
) : AwsOptions
{
    [CliOption("--recommender-config")]
    public string? RecommenderConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}