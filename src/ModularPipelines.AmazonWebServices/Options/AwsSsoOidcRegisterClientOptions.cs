using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-oidc", "register-client")]
public record AwsSsoOidcRegisterClientOptions(
[property: CliOption("--client-name")] string ClientName,
[property: CliOption("--client-type")] string ClientType
) : AwsOptions
{
    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}