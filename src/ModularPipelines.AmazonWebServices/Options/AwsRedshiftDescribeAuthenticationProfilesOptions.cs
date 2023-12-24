using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "describe-authentication-profiles")]
public record AwsRedshiftDescribeAuthenticationProfilesOptions : AwsOptions
{
    [CommandSwitch("--authentication-profile-name")]
    public string? AuthenticationProfileName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}