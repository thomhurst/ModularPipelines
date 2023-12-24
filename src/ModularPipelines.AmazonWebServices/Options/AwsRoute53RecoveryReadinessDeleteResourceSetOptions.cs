using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "delete-resource-set")]
public record AwsRoute53RecoveryReadinessDeleteResourceSetOptions(
[property: CommandSwitch("--resource-set-name")] string ResourceSetName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}