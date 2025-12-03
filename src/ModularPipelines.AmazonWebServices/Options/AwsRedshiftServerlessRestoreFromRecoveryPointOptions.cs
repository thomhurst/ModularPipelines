using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "restore-from-recovery-point")]
public record AwsRedshiftServerlessRestoreFromRecoveryPointOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--recovery-point-id")] string RecoveryPointId,
[property: CliOption("--workgroup-name")] string WorkgroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}