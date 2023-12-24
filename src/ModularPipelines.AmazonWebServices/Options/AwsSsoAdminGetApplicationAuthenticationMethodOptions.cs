using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "get-application-authentication-method")]
public record AwsSsoAdminGetApplicationAuthenticationMethodOptions(
[property: CommandSwitch("--application-arn")] string ApplicationArn,
[property: CommandSwitch("--authentication-method-type")] string AuthenticationMethodType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}