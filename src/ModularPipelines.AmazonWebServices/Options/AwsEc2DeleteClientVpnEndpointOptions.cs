using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-client-vpn-endpoint")]
public record AwsEc2DeleteClientVpnEndpointOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}