using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-client-vpn-endpoint")]
public record AwsEc2DeleteClientVpnEndpointOptions(
[property: CommandSwitch("--client-vpn-endpoint-id")] string ClientVpnEndpointId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}