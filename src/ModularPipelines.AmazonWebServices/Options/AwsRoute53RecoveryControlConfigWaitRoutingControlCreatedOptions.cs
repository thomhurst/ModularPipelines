using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-control-config", "wait", "routing-control-created")]
public record AwsRoute53RecoveryControlConfigWaitRoutingControlCreatedOptions(
[property: CliOption("--routing-control-arn")] string RoutingControlArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}