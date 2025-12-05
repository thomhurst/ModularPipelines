using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "put-application-authentication-method")]
public record AwsSsoAdminPutApplicationAuthenticationMethodOptions(
[property: CliOption("--application-arn")] string ApplicationArn,
[property: CliOption("--authentication-method")] string AuthenticationMethod,
[property: CliOption("--authentication-method-type")] string AuthenticationMethodType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}