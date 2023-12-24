using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-transit-gateway-vpc-attachment")]
public record AwsEc2ModifyTransitGatewayVpcAttachmentOptions(
[property: CommandSwitch("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId
) : AwsOptions
{
    [CommandSwitch("--add-subnet-ids")]
    public string[]? AddSubnetIds { get; set; }

    [CommandSwitch("--remove-subnet-ids")]
    public string[]? RemoveSubnetIds { get; set; }

    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}