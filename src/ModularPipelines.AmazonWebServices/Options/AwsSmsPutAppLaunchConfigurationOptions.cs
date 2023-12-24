using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sms", "put-app-launch-configuration")]
public record AwsSmsPutAppLaunchConfigurationOptions : AwsOptions
{
    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--role-name")]
    public string? RoleName { get; set; }

    [CommandSwitch("--server-group-launch-configurations")]
    public string[]? ServerGroupLaunchConfigurations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}