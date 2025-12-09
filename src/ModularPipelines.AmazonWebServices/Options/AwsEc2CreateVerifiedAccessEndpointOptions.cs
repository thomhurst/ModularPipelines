using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-verified-access-endpoint")]
public record AwsEc2CreateVerifiedAccessEndpointOptions(
[property: CliOption("--verified-access-group-id")] string VerifiedAccessGroupId,
[property: CliOption("--endpoint-type")] string EndpointType,
[property: CliOption("--attachment-type")] string AttachmentType,
[property: CliOption("--domain-certificate-arn")] string DomainCertificateArn,
[property: CliOption("--application-domain")] string ApplicationDomain,
[property: CliOption("--endpoint-domain-prefix")] string EndpointDomainPrefix
) : AwsOptions
{
    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--load-balancer-options")]
    public string? LoadBalancerOptions { get; set; }

    [CliOption("--network-interface-options")]
    public string? NetworkInterfaceOptions { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--policy-document")]
    public string? PolicyDocument { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}