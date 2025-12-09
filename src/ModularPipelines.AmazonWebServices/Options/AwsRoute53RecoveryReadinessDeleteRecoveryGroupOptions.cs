using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "delete-recovery-group")]
public record AwsRoute53RecoveryReadinessDeleteRecoveryGroupOptions(
[property: CliOption("--recovery-group-name")] string RecoveryGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}