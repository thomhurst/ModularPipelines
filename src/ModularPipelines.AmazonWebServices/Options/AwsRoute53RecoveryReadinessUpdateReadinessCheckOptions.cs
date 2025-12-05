using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "update-readiness-check")]
public record AwsRoute53RecoveryReadinessUpdateReadinessCheckOptions(
[property: CliOption("--readiness-check-name")] string ReadinessCheckName,
[property: CliOption("--resource-set-name")] string ResourceSetName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}