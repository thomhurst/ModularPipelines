using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "describe-vpc-endpoints")]
public record AwsEsDescribeVpcEndpointsOptions(
[property: CommandSwitch("--vpc-endpoint-ids")] string[] VpcEndpointIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}