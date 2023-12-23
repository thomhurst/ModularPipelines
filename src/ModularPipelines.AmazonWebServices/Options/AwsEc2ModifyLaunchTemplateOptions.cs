using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-launch-template")]
public record AwsEc2ModifyLaunchTemplateOptions : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--launch-template-id")]
    public string? LaunchTemplateId { get; set; }

    [CommandSwitch("--launch-template-name")]
    public string? LaunchTemplateName { get; set; }

    [CommandSwitch("--default-version")]
    public string? DefaultVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}