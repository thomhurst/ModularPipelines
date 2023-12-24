using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-authentication-profile")]
public record AwsRedshiftCreateAuthenticationProfileOptions(
[property: CommandSwitch("--authentication-profile-name")] string AuthenticationProfileName,
[property: CommandSwitch("--authentication-profile-content")] string AuthenticationProfileContent
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}