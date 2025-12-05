using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "add-flow-vpc-interfaces")]
public record AwsMediaconnectAddFlowVpcInterfacesOptions(
[property: CliOption("--flow-arn")] string FlowArn,
[property: CliOption("--vpc-interfaces")] string[] VpcInterfaces
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}