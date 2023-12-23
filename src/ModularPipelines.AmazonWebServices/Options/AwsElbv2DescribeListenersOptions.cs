using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "describe-listeners")]
public record AwsElbv2DescribeListenersOptions : AwsOptions
{
    [CommandSwitch("--load-balancer-arn")]
    public string? LoadBalancerArn { get; set; }

    [CommandSwitch("--listener-arns")]
    public string[]? ListenerArns { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}