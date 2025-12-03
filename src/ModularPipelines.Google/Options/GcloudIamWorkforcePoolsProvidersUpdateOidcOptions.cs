using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "providers", "update-oidc")]
public record GcloudIamWorkforcePoolsProvidersUpdateOidcOptions(
[property: CliArgument] string Provider,
[property: CliArgument] string Location,
[property: CliArgument] string WorkforcePool
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--attribute-condition")]
    public string? AttributeCondition { get; set; }

    [CliOption("--attribute-mapping")]
    public IEnumerable<KeyValue>? AttributeMapping { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--issuer-uri")]
    public string? IssuerUri { get; set; }

    [CliOption("--jwk-json-path")]
    public string? JwkJsonPath { get; set; }

    [CliOption("--web-sso-additional-scopes")]
    public string[]? WebSsoAdditionalScopes { get; set; }

    [CliOption("--web-sso-assertion-claims-behavior")]
    public string? WebSsoAssertionClaimsBehavior { get; set; }

    [CliOption("--web-sso-response-type")]
    public string? WebSsoResponseType { get; set; }

    [CliFlag("--clear-client-secret")]
    public bool? ClearClientSecret { get; set; }

    [CliOption("--client-secret-value")]
    public string? ClientSecretValue { get; set; }
}