using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-instance-metadata-options")]
public record AwsEc2ModifyInstanceMetadataOptionsOptions(
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--http-tokens")]
    public string? HttpTokens { get; set; }

    [CommandSwitch("--http-put-response-hop-limit")]
    public int? HttpPutResponseHopLimit { get; set; }

    [CommandSwitch("--http-endpoint")]
    public string? HttpEndpoint { get; set; }

    [CommandSwitch("--http-protocol-ipv6")]
    public string? HttpProtocolIpv6 { get; set; }

    [CommandSwitch("--instance-metadata-tags")]
    public string? InstanceMetadataTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}