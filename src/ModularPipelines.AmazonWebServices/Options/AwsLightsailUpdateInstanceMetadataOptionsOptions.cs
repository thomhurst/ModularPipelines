using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "update-instance-metadata-options")]
public record AwsLightsailUpdateInstanceMetadataOptionsOptions(
[property: CommandSwitch("--instance-name")] string InstanceName
) : AwsOptions
{
    [CommandSwitch("--http-tokens")]
    public string? HttpTokens { get; set; }

    [CommandSwitch("--http-endpoint")]
    public string? HttpEndpoint { get; set; }

    [CommandSwitch("--http-put-response-hop-limit")]
    public int? HttpPutResponseHopLimit { get; set; }

    [CommandSwitch("--http-protocol-ipv6")]
    public string? HttpProtocolIpv6 { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}