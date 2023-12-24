using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-control-config", "update-routing-control")]
public record AwsRoute53RecoveryControlConfigUpdateRoutingControlOptions(
[property: CommandSwitch("--routing-control-arn")] string RoutingControlArn,
[property: CommandSwitch("--routing-control-name")] string RoutingControlName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}