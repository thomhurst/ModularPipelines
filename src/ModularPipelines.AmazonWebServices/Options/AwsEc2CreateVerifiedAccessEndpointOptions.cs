using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-verified-access-endpoint")]
public record AwsEc2CreateVerifiedAccessEndpointOptions(
[property: CommandSwitch("--verified-access-group-id")] string VerifiedAccessGroupId,
[property: CommandSwitch("--endpoint-type")] string EndpointType,
[property: CommandSwitch("--attachment-type")] string AttachmentType,
[property: CommandSwitch("--domain-certificate-arn")] string DomainCertificateArn,
[property: CommandSwitch("--application-domain")] string ApplicationDomain,
[property: CommandSwitch("--endpoint-domain-prefix")] string EndpointDomainPrefix
) : AwsOptions
{
    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--load-balancer-options")]
    public string? LoadBalancerOptions { get; set; }

    [CommandSwitch("--network-interface-options")]
    public string? NetworkInterfaceOptions { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--policy-document")]
    public string? PolicyDocument { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}