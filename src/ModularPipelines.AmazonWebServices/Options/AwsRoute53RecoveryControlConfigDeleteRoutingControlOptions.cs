using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config", "delete-routing-control")]
public record AwsRoute53RecoveryControlConfigDeleteRoutingControlOptions(
[property: CliOption("--routing-control-arn")] string RoutingControlArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}