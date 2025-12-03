using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-cluster", "get-routing-control-state")]
public record AwsRoute53RecoveryClusterGetRoutingControlStateOptions(
[property: CliOption("--routing-control-arn")] string RoutingControlArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}