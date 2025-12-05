using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sms", "get-app-launch-configuration")]
public record AwsSmsGetAppLaunchConfigurationOptions : AwsOptions
{
    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}