using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "delete-recipe-version")]
public record AwsDatabrewDeleteRecipeVersionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--recipe-version")] string RecipeVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}