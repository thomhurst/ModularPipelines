using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "get-application-authentication-method")]
public record AwsSsoAdminGetApplicationAuthenticationMethodOptions(
[property: CliOption("--application-arn")] string ApplicationArn,
[property: CliOption("--authentication-method-type")] string AuthenticationMethodType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}