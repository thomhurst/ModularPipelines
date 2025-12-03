using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "remove-flow-vpc-interface")]
public record AwsMediaconnectRemoveFlowVpcInterfaceOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--vpc-interface-name")] string VpcInterfaceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}