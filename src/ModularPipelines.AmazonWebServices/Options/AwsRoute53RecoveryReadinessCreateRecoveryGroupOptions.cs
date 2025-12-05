using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "create-recovery-group")]
public record AwsRoute53RecoveryReadinessCreateRecoveryGroupOptions(
[property: CliOption("--recovery-group-name")] string RecoveryGroupName
) : AwsOptions
{
    [CliOption("--cells")]
    public string[]? Cells { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}