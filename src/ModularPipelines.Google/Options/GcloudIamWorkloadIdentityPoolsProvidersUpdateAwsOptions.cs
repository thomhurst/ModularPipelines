using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "providers", "update-aws")]
public record GcloudIamWorkloadIdentityPoolsProvidersUpdateAwsOptions(
[property: CliArgument] string Provider,
[property: CliArgument] string Location,
[property: CliArgument] string WorkloadIdentityPool
) : GcloudOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--attribute-condition")]
    public string? AttributeCondition { get; set; }

    [CliOption("--attribute-mapping")]
    public IEnumerable<KeyValue>? AttributeMapping { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}