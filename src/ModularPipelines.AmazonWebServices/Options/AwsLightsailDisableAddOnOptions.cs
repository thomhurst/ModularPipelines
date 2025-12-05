using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "disable-add-on")]
public record AwsLightsailDisableAddOnOptions(
[property: CliOption("--add-on-type")] string AddOnType,
[property: CliOption("--resource-name")] string ResourceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}