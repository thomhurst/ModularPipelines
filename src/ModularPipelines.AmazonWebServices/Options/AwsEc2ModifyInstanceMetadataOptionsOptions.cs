using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-instance-metadata-options")]
public record AwsEc2ModifyInstanceMetadataOptionsOptions(
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--http-tokens")]
    public string? HttpTokens { get; set; }

    [CliOption("--http-put-response-hop-limit")]
    public int? HttpPutResponseHopLimit { get; set; }

    [CliOption("--http-endpoint")]
    public string? HttpEndpoint { get; set; }

    [CliOption("--http-protocol-ipv6")]
    public string? HttpProtocolIpv6 { get; set; }

    [CliOption("--instance-metadata-tags")]
    public string? InstanceMetadataTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}