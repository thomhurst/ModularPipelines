using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "describe-recipe")]
public record AwsDatabrewDescribeRecipeOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--recipe-version")]
    public string? RecipeVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}