using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "close-instance-public-ports")]
public record AwsLightsailCloseInstancePublicPortsOptions(
[property: CommandSwitch("--port-info")] string PortInfo,
[property: CommandSwitch("--instance-name")] string InstanceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}