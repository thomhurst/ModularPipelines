using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "open-instance-public-ports")]
public record AwsLightsailOpenInstancePublicPortsOptions(
[property: CliOption("--port-info")] string PortInfo,
[property: CliOption("--instance-name")] string InstanceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}