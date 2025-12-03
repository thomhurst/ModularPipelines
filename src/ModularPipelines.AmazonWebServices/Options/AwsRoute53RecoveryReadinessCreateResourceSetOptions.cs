using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "create-resource-set")]
public record AwsRoute53RecoveryReadinessCreateResourceSetOptions(
[property: CliOption("--resource-set-name")] string ResourceSetName,
[property: CliOption("--resource-set-type")] string ResourceSetType,
[property: CliOption("--resources")] string[] Resources
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}