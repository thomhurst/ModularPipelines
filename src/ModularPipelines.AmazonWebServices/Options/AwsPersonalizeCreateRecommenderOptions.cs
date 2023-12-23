using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-recommender")]
public record AwsPersonalizeCreateRecommenderOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--dataset-group-arn")] string DatasetGroupArn,
[property: CommandSwitch("--recipe-arn")] string RecipeArn
) : AwsOptions
{
    [CommandSwitch("--recommender-config")]
    public string? RecommenderConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}