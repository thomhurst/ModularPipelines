using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrassv2", "get-component")]
public record AwsGreengrassv2GetComponentOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--recipe-output-format")]
    public string? RecipeOutputFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}