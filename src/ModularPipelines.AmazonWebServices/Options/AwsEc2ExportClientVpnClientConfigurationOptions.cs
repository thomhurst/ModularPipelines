using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "export-client-vpn-client-configuration")]
public record AwsEc2ExportClientVpnClientConfigurationOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}