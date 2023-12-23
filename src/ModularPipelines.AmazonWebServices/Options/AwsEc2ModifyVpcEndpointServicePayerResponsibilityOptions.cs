using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpc-endpoint-service-payer-responsibility")]
public record AwsEc2ModifyVpcEndpointServicePayerResponsibilityOptions(
[property: CommandSwitch("--service-id")] string ServiceId,
[property: CommandSwitch("--payer-responsibility")] string PayerResponsibility
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}