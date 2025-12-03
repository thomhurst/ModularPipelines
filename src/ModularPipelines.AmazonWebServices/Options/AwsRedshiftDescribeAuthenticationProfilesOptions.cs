using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-authentication-profiles")]
public record AwsRedshiftDescribeAuthenticationProfilesOptions : AwsOptions
{
    [CliOption("--authentication-profile-name")]
    public string? AuthenticationProfileName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}