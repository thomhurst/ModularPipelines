using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sms", "get-app-launch-configuration")]
public record AwsSmsGetAppLaunchConfigurationOptions : AwsOptions
{
    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}