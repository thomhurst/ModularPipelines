using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sms", "put-app-launch-configuration")]
public record AwsSmsPutAppLaunchConfigurationOptions : AwsOptions
{
    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--role-name")]
    public string? RoleName { get; set; }

    [CliOption("--server-group-launch-configurations")]
    public string[]? ServerGroupLaunchConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}