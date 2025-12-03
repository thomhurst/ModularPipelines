using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("drs", "delete-launch-configuration-template")]
public record AwsDrsDeleteLaunchConfigurationTemplateOptions(
[property: CliOption("--launch-configuration-template-id")] string LaunchConfigurationTemplateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}