using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "delete-recipe-version")]
public record AwsDatabrewDeleteRecipeVersionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--recipe-version")] string RecipeVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}