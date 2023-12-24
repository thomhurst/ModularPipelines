using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-control-config", "wait", "routing-control-deleted")]
public record AwsRoute53RecoveryControlConfigWaitRoutingControlDeletedOptions(
[property: CommandSwitch("--routing-control-arn")] string RoutingControlArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}