using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "update-workload-share")]
public record AwsWellarchitectedUpdateWorkloadShareOptions(
[property: CliOption("--share-id")] string ShareId,
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--permission-type")] string PermissionType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}