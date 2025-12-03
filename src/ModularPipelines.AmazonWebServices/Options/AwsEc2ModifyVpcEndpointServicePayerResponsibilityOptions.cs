using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpc-endpoint-service-payer-responsibility")]
public record AwsEc2ModifyVpcEndpointServicePayerResponsibilityOptions(
[property: CliOption("--service-id")] string ServiceId,
[property: CliOption("--payer-responsibility")] string PayerResponsibility
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}