using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "providers", "list")]
public record GcloudIamWorkforcePoolsProvidersListOptions(
[property: CliOption("--workforce-pool")] string WorkforcePool,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliFlag("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}