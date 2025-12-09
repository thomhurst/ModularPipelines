using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "get-container-recipe-policy")]
public record AwsImagebuilderGetContainerRecipePolicyOptions(
[property: CliOption("--container-recipe-arn")] string ContainerRecipeArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}