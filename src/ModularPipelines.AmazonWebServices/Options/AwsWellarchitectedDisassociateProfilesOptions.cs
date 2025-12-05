using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "disassociate-profiles")]
public record AwsWellarchitectedDisassociateProfilesOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--profile-arns")] string[] ProfileArns
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}