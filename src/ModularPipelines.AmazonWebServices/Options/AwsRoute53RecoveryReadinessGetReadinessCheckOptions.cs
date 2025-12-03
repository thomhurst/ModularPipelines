using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "get-readiness-check")]
public record AwsRoute53RecoveryReadinessGetReadinessCheckOptions(
[property: CliOption("--readiness-check-name")] string ReadinessCheckName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}