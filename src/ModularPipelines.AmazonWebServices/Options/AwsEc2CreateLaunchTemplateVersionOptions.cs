using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-launch-template-version")]
public record AwsEc2CreateLaunchTemplateVersionOptions(
[property: CommandSwitch("--launch-template-data")] string LaunchTemplateData
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--launch-template-id")]
    public string? LaunchTemplateId { get; set; }

    [CommandSwitch("--launch-template-name")]
    public string? LaunchTemplateName { get; set; }

    [CommandSwitch("--source-version")]
    public string? SourceVersion { get; set; }

    [CommandSwitch("--version-description")]
    public string? VersionDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}