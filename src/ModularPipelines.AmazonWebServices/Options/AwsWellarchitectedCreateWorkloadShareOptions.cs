using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "create-workload-share")]
public record AwsWellarchitectedCreateWorkloadShareOptions(
[property: CliOption("--workload-id")] string WorkloadId,
[property: CliOption("--shared-with")] string SharedWith,
[property: CliOption("--permission-type")] string PermissionType
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}