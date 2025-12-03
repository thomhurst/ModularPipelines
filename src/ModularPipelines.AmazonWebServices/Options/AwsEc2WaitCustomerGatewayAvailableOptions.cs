using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "wait", "customer-gateway-available")]
public record AwsEc2WaitCustomerGatewayAvailableOptions : AwsOptions
{
    [CliOption("--customer-gateway-ids")]
    public string[]? CustomerGatewayIds { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}