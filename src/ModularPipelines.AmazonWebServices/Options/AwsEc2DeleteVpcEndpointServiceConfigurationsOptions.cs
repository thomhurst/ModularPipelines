using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-vpc-endpoint-service-configurations")]
public record AwsEc2DeleteVpcEndpointServiceConfigurationsOptions(
[property: CommandSwitch("--service-ids")] string[] ServiceIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}