using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config", "update-routing-control")]
public record AwsRoute53RecoveryControlConfigUpdateRoutingControlOptions(
[property: CliOption("--routing-control-arn")] string RoutingControlArn,
[property: CliOption("--routing-control-name")] string RoutingControlName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}