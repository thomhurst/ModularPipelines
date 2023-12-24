using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "export-client-vpn-client-configuration")]
public record AwsEc2ExportClientVpnClientConfigurationOptions(
[property: CommandSwitch("--client-vpn-endpoint-id")] string ClientVpnEndpointId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}