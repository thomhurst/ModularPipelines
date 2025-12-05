using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-solution")]
public record AwsPersonalizeCreateSolutionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--dataset-group-arn")] string DatasetGroupArn
) : AwsOptions
{
    [CliOption("--recipe-arn")]
    public string? RecipeArn { get; set; }

    [CliOption("--event-type")]
    public string? EventType { get; set; }

    [CliOption("--solution-config")]
    public string? SolutionConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}