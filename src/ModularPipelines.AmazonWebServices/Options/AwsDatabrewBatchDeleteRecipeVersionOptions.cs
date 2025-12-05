using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "batch-delete-recipe-version")]
public record AwsDatabrewBatchDeleteRecipeVersionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--recipe-versions")] string[] RecipeVersions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}