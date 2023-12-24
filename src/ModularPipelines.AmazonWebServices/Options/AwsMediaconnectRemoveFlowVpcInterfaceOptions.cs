using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "remove-flow-vpc-interface")]
public record AwsMediaconnectRemoveFlowVpcInterfaceOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--vpc-interface-name")] string VpcInterfaceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}