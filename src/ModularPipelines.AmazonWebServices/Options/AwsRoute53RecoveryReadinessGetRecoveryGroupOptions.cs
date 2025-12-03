using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "get-recovery-group")]
public record AwsRoute53RecoveryReadinessGetRecoveryGroupOptions(
[property: CliOption("--recovery-group-name")] string RecoveryGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}