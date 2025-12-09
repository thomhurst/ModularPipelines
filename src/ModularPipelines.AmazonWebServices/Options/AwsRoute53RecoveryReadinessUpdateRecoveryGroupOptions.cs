using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "update-recovery-group")]
public record AwsRoute53RecoveryReadinessUpdateRecoveryGroupOptions(
[property: CliOption("--cells")] string[] Cells,
[property: CliOption("--recovery-group-name")] string RecoveryGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}