using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-oidc", "start-device-authorization")]
public record AwsSsoOidcStartDeviceAuthorizationOptions(
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--client-secret")] string ClientSecret,
[property: CliOption("--start-url")] string StartUrl
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}