using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "providers", "create-oidc")]
public record GcloudIamWorkforcePoolsProvidersCreateOidcOptions(
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string WorkforcePool,
[property: CommandSwitch("--attribute-mapping")] IEnumerable<KeyValue> AttributeMapping,
[property: CommandSwitch("--client-id")] string ClientId,
[property: CommandSwitch("--issuer-uri")] string IssuerUri,
[property: CommandSwitch("--web-sso-assertion-claims-behavior")] string WebSsoAssertionClaimsBehavior,
[property: CommandSwitch("--web-sso-response-type")] string WebSsoResponseType,
[property: CommandSwitch("--web-sso-additional-scopes")] string[] WebSsoAdditionalScopes
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--attribute-condition")]
    public string? AttributeCondition { get; set; }

    [CommandSwitch("--client-secret-value")]
    public string? ClientSecretValue { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--jwk-json-path")]
    public string? JwkJsonPath { get; set; }
}