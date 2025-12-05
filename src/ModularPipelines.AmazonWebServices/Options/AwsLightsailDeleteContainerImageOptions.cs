using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "delete-container-image")]
public record AwsLightsailDeleteContainerImageOptions(
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--image")] string Image
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}