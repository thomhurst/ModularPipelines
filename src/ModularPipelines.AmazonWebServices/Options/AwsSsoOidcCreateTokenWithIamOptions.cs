using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-oidc", "create-token-with-iam")]
public record AwsSsoOidcCreateTokenWithIamOptions(
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--grant-type")] string GrantType
) : AwsOptions
{
    [CliOption("--code")]
    public string? Code { get; set; }

    [CliOption("--refresh-token")]
    public string? RefreshToken { get; set; }

    [CliOption("--assertion")]
    public string? Assertion { get; set; }

    [CliOption("--scope")]
    public string[]? Scope { get; set; }

    [CliOption("--redirect-uri")]
    public string? RedirectUri { get; set; }

    [CliOption("--subject-token")]
    public string? SubjectToken { get; set; }

    [CliOption("--subject-token-type")]
    public string? SubjectTokenType { get; set; }

    [CliOption("--requested-token-type")]
    public string? RequestedTokenType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}