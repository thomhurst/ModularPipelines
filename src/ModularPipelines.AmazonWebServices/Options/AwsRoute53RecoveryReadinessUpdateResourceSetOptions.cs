using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "update-resource-set")]
public record AwsRoute53RecoveryReadinessUpdateResourceSetOptions(
[property: CommandSwitch("--resource-set-name")] string ResourceSetName,
[property: CommandSwitch("--resource-set-type")] string ResourceSetType,
[property: CommandSwitch("--resources")] string[] Resources
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}