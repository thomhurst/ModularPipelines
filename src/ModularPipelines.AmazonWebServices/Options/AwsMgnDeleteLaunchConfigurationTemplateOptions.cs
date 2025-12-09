using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "delete-launch-configuration-template")]
public record AwsMgnDeleteLaunchConfigurationTemplateOptions(
[property: CliOption("--launch-configuration-template-id")] string LaunchConfigurationTemplateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}