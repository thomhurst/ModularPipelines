using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-control-config", "describe-routing-control")]
public record AwsRoute53RecoveryControlConfigDescribeRoutingControlOptions(
[property: CommandSwitch("--routing-control-arn")] string RoutingControlArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}