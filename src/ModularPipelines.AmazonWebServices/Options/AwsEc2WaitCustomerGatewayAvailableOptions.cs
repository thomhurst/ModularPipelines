using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "wait", "customer-gateway-available")]
public record AwsEc2WaitCustomerGatewayAvailableOptions : AwsOptions
{
    [CommandSwitch("--customer-gateway-ids")]
    public string[]? CustomerGatewayIds { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}