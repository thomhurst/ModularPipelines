using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "providers", "update-oidc")]
public record GcloudIamWorkforcePoolsProvidersUpdateOidcOptions(
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string WorkforcePool
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--attribute-condition")]
    public string? AttributeCondition { get; set; }

    [CommandSwitch("--attribute-mapping")]
    public IEnumerable<KeyValue>? AttributeMapping { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--issuer-uri")]
    public string? IssuerUri { get; set; }

    [CommandSwitch("--jwk-json-path")]
    public string? JwkJsonPath { get; set; }

    [CommandSwitch("--web-sso-additional-scopes")]
    public string[]? WebSsoAdditionalScopes { get; set; }

    [CommandSwitch("--web-sso-assertion-claims-behavior")]
    public string? WebSsoAssertionClaimsBehavior { get; set; }

    [CommandSwitch("--web-sso-response-type")]
    public string? WebSsoResponseType { get; set; }

    [BooleanCommandSwitch("--clear-client-secret")]
    public bool? ClearClientSecret { get; set; }

    [CommandSwitch("--client-secret-value")]
    public string? ClientSecretValue { get; set; }
}