using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "set-ip-address-type")]
public record AwsLightsailSetIpAddressTypeOptions(
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--ip-address-type")] string IpAddressType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}