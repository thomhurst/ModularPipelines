using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "update-resource-set")]
public record AwsRoute53RecoveryReadinessUpdateResourceSetOptions(
[property: CliOption("--resource-set-name")] string ResourceSetName,
[property: CliOption("--resource-set-type")] string ResourceSetType,
[property: CliOption("--resources")] string[] Resources
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}