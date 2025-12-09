using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "upgrade-lens-review")]
public record AwsWellarchitectedUpgradeLensReviewOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--lens-alias")] string LensAlias,
[property: CliOption("--milestone-name")] string MilestoneName
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}