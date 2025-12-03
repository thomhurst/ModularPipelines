using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "describe-ssl-policies")]
public record AwsElbv2DescribeSslPoliciesOptions : AwsOptions
{
    [CliOption("--names")]
    public string[]? Names { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--load-balancer-type")]
    public string? LoadBalancerType { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}