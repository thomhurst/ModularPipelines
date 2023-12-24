using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "delete-view-version")]
public record AwsConnectDeleteViewVersionOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--view-id")] string ViewId,
[property: CommandSwitch("--view-version")] int ViewVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}