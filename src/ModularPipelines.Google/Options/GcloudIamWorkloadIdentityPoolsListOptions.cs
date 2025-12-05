using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "list")]
public record GcloudIamWorkloadIdentityPoolsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliFlag("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}