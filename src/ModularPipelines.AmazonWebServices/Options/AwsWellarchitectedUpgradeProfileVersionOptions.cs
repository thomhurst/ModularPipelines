using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "upgrade-profile-version")]
public record AwsWellarchitectedUpgradeProfileVersionOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--profile-arn")] string ProfileArn
) : AwsOptions
{
    [CliOption("--milestone-name")]
    public string? MilestoneName { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}