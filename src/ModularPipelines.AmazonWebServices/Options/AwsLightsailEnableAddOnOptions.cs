using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "enable-add-on")]
public record AwsLightsailEnableAddOnOptions(
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--add-on-request")] string AddOnRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}