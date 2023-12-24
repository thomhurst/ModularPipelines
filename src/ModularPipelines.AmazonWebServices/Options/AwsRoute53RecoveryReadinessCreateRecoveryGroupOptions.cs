using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "create-recovery-group")]
public record AwsRoute53RecoveryReadinessCreateRecoveryGroupOptions(
[property: CommandSwitch("--recovery-group-name")] string RecoveryGroupName
) : AwsOptions
{
    [CommandSwitch("--cells")]
    public string[]? Cells { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}