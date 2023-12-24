using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-control-config", "wait", "routing-control-created")]
public record AwsRoute53RecoveryControlConfigWaitRoutingControlCreatedOptions(
[property: CommandSwitch("--routing-control-arn")] string RoutingControlArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}