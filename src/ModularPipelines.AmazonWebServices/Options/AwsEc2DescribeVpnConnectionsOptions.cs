using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-vpn-connections")]
public record AwsEc2DescribeVpnConnectionsOptions : AwsOptions
{
    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--vpn-connection-ids")]
    public string[]? VpnConnectionIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}