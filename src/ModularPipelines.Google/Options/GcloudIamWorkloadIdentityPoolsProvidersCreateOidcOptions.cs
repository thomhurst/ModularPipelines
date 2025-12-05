using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "providers", "create-oidc")]
public record GcloudIamWorkloadIdentityPoolsProvidersCreateOidcOptions(
[property: CliArgument] string Provider,
[property: CliArgument] string Location,
[property: CliArgument] string WorkloadIdentityPool,
[property: CliOption("--attribute-mapping")] IEnumerable<KeyValue> AttributeMapping,
[property: CliOption("--issuer-uri")] string IssuerUri
) : GcloudOptions
{
    [CliOption("--allowed-audiences")]
    public string[]? AllowedAudiences { get; set; }

    [CliOption("--attribute-condition")]
    public string? AttributeCondition { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--jwk-json-path")]
    public string? JwkJsonPath { get; set; }
}