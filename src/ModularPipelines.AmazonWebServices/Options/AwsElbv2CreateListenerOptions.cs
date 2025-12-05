using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "create-listener")]
public record AwsElbv2CreateListenerOptions(
[property: CliOption("--load-balancer-arn")] string LoadBalancerArn,
[property: CliOption("--default-actions")] string[] DefaultActions
) : AwsOptions
{
    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [CliOption("--certificates")]
    public string[]? Certificates { get; set; }

    [CliOption("--alpn-policy")]
    public string[]? AlpnPolicy { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--mutual-authentication")]
    public string? MutualAuthentication { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}