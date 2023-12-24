using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-oidc", "create-token-with-iam")]
public record AwsSsoOidcCreateTokenWithIamOptions(
[property: CommandSwitch("--client-id")] string ClientId,
[property: CommandSwitch("--grant-type")] string GrantType
) : AwsOptions
{
    [CommandSwitch("--code")]
    public string? Code { get; set; }

    [CommandSwitch("--refresh-token")]
    public string? RefreshToken { get; set; }

    [CommandSwitch("--assertion")]
    public string? Assertion { get; set; }

    [CommandSwitch("--scope")]
    public string[]? Scope { get; set; }

    [CommandSwitch("--redirect-uri")]
    public string? RedirectUri { get; set; }

    [CommandSwitch("--subject-token")]
    public string? SubjectToken { get; set; }

    [CommandSwitch("--subject-token-type")]
    public string? SubjectTokenType { get; set; }

    [CommandSwitch("--requested-token-type")]
    public string? RequestedTokenType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}