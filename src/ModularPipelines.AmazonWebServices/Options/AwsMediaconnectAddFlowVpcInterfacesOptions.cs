using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "add-flow-vpc-interfaces")]
public record AwsMediaconnectAddFlowVpcInterfacesOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--vpc-interfaces")] string[] VpcInterfaces
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}