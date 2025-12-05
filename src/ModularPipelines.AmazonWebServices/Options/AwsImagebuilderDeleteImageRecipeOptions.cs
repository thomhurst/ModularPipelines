using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "delete-image-recipe")]
public record AwsImagebuilderDeleteImageRecipeOptions(
[property: CliOption("--image-recipe-arn")] string ImageRecipeArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}