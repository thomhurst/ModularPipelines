using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "create")]
public record GcloudIamWorkloadIdentityPoolsCreateOptions(
[property: CliArgument] string WorkloadIdentityPool,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}