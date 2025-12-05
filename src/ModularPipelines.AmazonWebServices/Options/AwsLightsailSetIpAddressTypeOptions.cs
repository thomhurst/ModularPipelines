using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "set-ip-address-type")]
public record AwsLightsailSetIpAddressTypeOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--ip-address-type")] string IpAddressType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}