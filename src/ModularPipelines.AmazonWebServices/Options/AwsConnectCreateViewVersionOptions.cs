using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-view-version")]
public record AwsConnectCreateViewVersionOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--view-id")] string ViewId
) : AwsOptions
{
    [CommandSwitch("--version-description")]
    public string? VersionDescription { get; set; }

    [CommandSwitch("--view-content-sha256")]
    public string? ViewContentSha256 { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}