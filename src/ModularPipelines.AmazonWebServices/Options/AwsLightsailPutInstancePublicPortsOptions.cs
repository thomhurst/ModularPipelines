using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "put-instance-public-ports")]
public record AwsLightsailPutInstancePublicPortsOptions(
[property: CliOption("--port-infos")] string[] PortInfos,
[property: CliOption("--instance-name")] string InstanceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}