using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "update-instance-metadata-options")]
public record AwsLightsailUpdateInstanceMetadataOptionsOptions(
[property: CliOption("--instance-name")] string InstanceName
) : AwsOptions
{
    [CliOption("--http-tokens")]
    public string? HttpTokens { get; set; }

    [CliOption("--http-endpoint")]
    public string? HttpEndpoint { get; set; }

    [CliOption("--http-put-response-hop-limit")]
    public int? HttpPutResponseHopLimit { get; set; }

    [CliOption("--http-protocol-ipv6")]
    public string? HttpProtocolIpv6 { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}