using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-application")]
public record AwsAppstreamCreateApplicationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--icon-s3-location")] string IconS3Location,
[property: CommandSwitch("--launch-path")] string LaunchPath,
[property: CommandSwitch("--platforms")] string[] Platforms,
[property: CommandSwitch("--instance-families")] string[] InstanceFamilies,
[property: CommandSwitch("--app-block-arn")] string AppBlockArn
) : AwsOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--working-directory")]
    public string? AwsAppsWorkingDirectory { get; set; }

    [CommandSwitch("--launch-parameters")]
    public string? LaunchParameters { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}