using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "enable-fast-launch")]
public record AwsEc2EnableFastLaunchOptions(
[property: CommandSwitch("--image-id")] string ImageId
) : AwsOptions
{
    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--snapshot-configuration")]
    public string? SnapshotConfiguration { get; set; }

    [CommandSwitch("--launch-template")]
    public string? LaunchTemplate { get; set; }

    [CommandSwitch("--max-parallel-launches")]
    public int? MaxParallelLaunches { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}