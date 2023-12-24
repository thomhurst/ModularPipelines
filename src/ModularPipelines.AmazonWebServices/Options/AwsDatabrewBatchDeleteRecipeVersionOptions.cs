using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "batch-delete-recipe-version")]
public record AwsDatabrewBatchDeleteRecipeVersionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--recipe-versions")] string[] RecipeVersions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}