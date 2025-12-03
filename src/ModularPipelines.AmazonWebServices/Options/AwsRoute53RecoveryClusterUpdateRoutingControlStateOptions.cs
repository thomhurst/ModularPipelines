using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-cluster", "update-routing-control-state")]
public record AwsRoute53RecoveryClusterUpdateRoutingControlStateOptions(
[property: CliOption("--routing-control-arn")] string RoutingControlArn,
[property: CliOption("--routing-control-state")] string RoutingControlState
) : AwsOptions
{
    [CliOption("--safety-rules-to-override")]
    public string[]? SafetyRulesToOverride { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}