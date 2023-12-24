using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "describe-vpc-endpoints")]
public record AwsOpensearchDescribeVpcEndpointsOptions(
[property: CommandSwitch("--vpc-endpoint-ids")] string[] VpcEndpointIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}