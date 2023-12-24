using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "update-application")]
public record AwsAppstreamUpdateApplicationOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--icon-s3-location")]
    public string? IconS3Location { get; set; }

    [CommandSwitch("--launch-path")]
    public string? LaunchPath { get; set; }

    [CommandSwitch("--working-directory")]
    public string? AwsAppsWorkingDirectory { get; set; }

    [CommandSwitch("--launch-parameters")]
    public string? LaunchParameters { get; set; }

    [CommandSwitch("--app-block-arn")]
    public string? AppBlockArn { get; set; }

    [CommandSwitch("--attributes-to-delete")]
    public string[]? AttributesToDelete { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}