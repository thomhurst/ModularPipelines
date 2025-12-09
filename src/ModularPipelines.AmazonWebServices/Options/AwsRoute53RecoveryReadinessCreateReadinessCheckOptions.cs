using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "create-readiness-check")]
public record AwsRoute53RecoveryReadinessCreateReadinessCheckOptions(
[property: CliOption("--readiness-check-name")] string ReadinessCheckName,
[property: CliOption("--resource-set-name")] string ResourceSetName
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}