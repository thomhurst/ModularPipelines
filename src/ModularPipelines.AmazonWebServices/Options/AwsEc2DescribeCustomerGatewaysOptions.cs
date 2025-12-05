using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-customer-gateways")]
public record AwsEc2DescribeCustomerGatewaysOptions : AwsOptions
{
    [CliOption("--customer-gateway-ids")]
    public string[]? CustomerGatewayIds { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}