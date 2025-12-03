using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config", "create-routing-control")]
public record AwsRoute53RecoveryControlConfigCreateRoutingControlOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--routing-control-name")] string RoutingControlName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--control-panel-arn")]
    public string? ControlPanelArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}