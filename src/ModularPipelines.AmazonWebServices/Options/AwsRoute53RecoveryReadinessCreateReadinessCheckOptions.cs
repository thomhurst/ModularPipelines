using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "create-readiness-check")]
public record AwsRoute53RecoveryReadinessCreateReadinessCheckOptions(
[property: CommandSwitch("--readiness-check-name")] string ReadinessCheckName,
[property: CommandSwitch("--resource-set-name")] string ResourceSetName
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}