using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-cluster", "update-routing-control-states")]
public record AwsRoute53RecoveryClusterUpdateRoutingControlStatesOptions(
[property: CliOption("--update-routing-control-state-entries")] string[] UpdateRoutingControlStateEntries
) : AwsOptions
{
    [CliOption("--safety-rules-to-override")]
    public string[]? SafetyRulesToOverride { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}