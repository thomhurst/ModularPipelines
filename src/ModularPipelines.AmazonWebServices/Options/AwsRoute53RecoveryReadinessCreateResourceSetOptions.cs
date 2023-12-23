using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "create-resource-set")]
public record AwsRoute53RecoveryReadinessCreateResourceSetOptions(
[property: CommandSwitch("--resource-set-name")] string ResourceSetName,
[property: CommandSwitch("--resource-set-type")] string ResourceSetType,
[property: CommandSwitch("--resources")] string[] Resources
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}