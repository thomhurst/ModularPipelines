using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-oidc", "create-token")]
public record AwsSsoOidcCreateTokenOptions(
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--client-secret")] string ClientSecret,
[property: CliOption("--grant-type")] string GrantType
) : AwsOptions
{
    [CliOption("--device-code")]
    public string? DeviceCode { get; set; }

    [CliOption("--code")]
    public string? Code { get; set; }

    [CliOption("--refresh-token")]
    public string? RefreshToken { get; set; }

    [CliOption("--scope")]
    public string[]? Scope { get; set; }

    [CliOption("--redirect-uri")]
    public string? RedirectUri { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}