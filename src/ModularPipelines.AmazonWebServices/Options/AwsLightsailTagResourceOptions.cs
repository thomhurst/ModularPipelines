using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "tag-resource")]
public record AwsLightsailTagResourceOptions(
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--tags")] string[] Tags
) : AwsOptions
{
    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}