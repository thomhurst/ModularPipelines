using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "delete-workload-share")]
public record AwsWellarchitectedDeleteWorkloadShareOptions(
[property: CliOption("--share-id")] string ShareId,
[property: CliOption("--workload-id")] string WorkloadId
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}