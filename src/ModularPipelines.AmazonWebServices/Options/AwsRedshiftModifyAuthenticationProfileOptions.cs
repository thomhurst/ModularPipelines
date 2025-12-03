using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-authentication-profile")]
public record AwsRedshiftModifyAuthenticationProfileOptions(
[property: CliOption("--authentication-profile-name")] string AuthenticationProfileName,
[property: CliOption("--authentication-profile-content")] string AuthenticationProfileContent
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}