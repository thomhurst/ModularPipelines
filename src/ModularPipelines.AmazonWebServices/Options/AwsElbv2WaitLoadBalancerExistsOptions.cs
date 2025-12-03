using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "wait", "load-balancer-exists")]
public record AwsElbv2WaitLoadBalancerExistsOptions : AwsOptions
{
    [CliOption("--load-balancer-arns")]
    public string[]? LoadBalancerArns { get; set; }

    [CliOption("--names")]
    public string[]? Names { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}