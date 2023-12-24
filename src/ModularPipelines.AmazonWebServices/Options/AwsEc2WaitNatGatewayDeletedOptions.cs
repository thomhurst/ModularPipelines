using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "wait", "nat-gateway-deleted")]
public record AwsEc2WaitNatGatewayDeletedOptions : AwsOptions
{
    [CommandSwitch("--filter")]
    public string[]? Filter { get; set; }

    [CommandSwitch("--nat-gateway-ids")]
    public string[]? NatGatewayIds { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}