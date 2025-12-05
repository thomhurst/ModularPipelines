using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "providers", "create-oidc")]
public record GcloudIamWorkforcePoolsProvidersCreateOidcOptions(
[property: CliArgument] string Provider,
[property: CliArgument] string Location,
[property: CliArgument] string WorkforcePool,
[property: CliOption("--attribute-mapping")] IEnumerable<KeyValue> AttributeMapping,
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--issuer-uri")] string IssuerUri,
[property: CliOption("--web-sso-assertion-claims-behavior")] string WebSsoAssertionClaimsBehavior,
[property: CliOption("--web-sso-response-type")] string WebSsoResponseType,
[property: CliOption("--web-sso-additional-scopes")] string[] WebSsoAdditionalScopes
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--attribute-condition")]
    public string? AttributeCondition { get; set; }

    [CliOption("--client-secret-value")]
    public string? ClientSecretValue { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--jwk-json-path")]
    public string? JwkJsonPath { get; set; }
}