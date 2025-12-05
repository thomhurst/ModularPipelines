using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "providers", "keys", "list")]
public record GcloudIamWorkforcePoolsProvidersKeysListOptions(
[property: CliOption("--provider")] string Provider,
[property: CliOption("--location")] string Location,
[property: CliOption("--workforce-pool")] string WorkforcePool
) : GcloudOptions
{
    [CliFlag("--show-deleted")]
    public bool? ShowDeleted { get; set; }
}