using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "register-task-definition")]
public record AwsEcsRegisterTaskDefinitionOptions(
[property: CommandSwitch("--family")] string Family,
[property: CommandSwitch("--container-definitions")] string[] ContainerDefinitions
) : AwsOptions
{
    [CommandSwitch("--task-role-arn")]
    public string? TaskRoleArn { get; set; }

    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CommandSwitch("--network-mode")]
    public string? NetworkMode { get; set; }

    [CommandSwitch("--volumes")]
    public string[]? Volumes { get; set; }

    [CommandSwitch("--placement-constraints")]
    public string[]? PlacementConstraints { get; set; }

    [CommandSwitch("--requires-compatibilities")]
    public string[]? RequiresCompatibilities { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--pid-mode")]
    public string? PidMode { get; set; }

    [CommandSwitch("--ipc-mode")]
    public string? IpcMode { get; set; }

    [CommandSwitch("--proxy-configuration")]
    public string? ProxyConfiguration { get; set; }

    [CommandSwitch("--inference-accelerators")]
    public string[]? InferenceAccelerators { get; set; }

    [CommandSwitch("--ephemeral-storage")]
    public string? EphemeralStorage { get; set; }

    [CommandSwitch("--runtime-platform")]
    public string? RuntimePlatform { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}