using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-container-services")]
public record AwsLightsailGetContainerServicesOptions : AwsOptions
{
    [CommandSwitch("--service-name")]
    public string? ServiceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}