using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "create-deployment")]
public record AwsOpsworksCreateDeploymentOptions(
[property: CommandSwitch("--stack-id")] string StackId,
[property: CommandSwitch("--command")] string Command
) : AwsOptions
{
    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--instance-ids")]
    public string[]? InstanceIds { get; set; }

    [CommandSwitch("--layer-ids")]
    public string[]? LayerIds { get; set; }

    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--custom-json")]
    public string? CustomJson { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}