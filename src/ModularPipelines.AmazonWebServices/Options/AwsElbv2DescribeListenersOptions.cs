using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "describe-listeners")]
public record AwsElbv2DescribeListenersOptions : AwsOptions
{
    [CliOption("--load-balancer-arn")]
    public string? LoadBalancerArn { get; set; }

    [CliOption("--listener-arns")]
    public string[]? ListenerArns { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}