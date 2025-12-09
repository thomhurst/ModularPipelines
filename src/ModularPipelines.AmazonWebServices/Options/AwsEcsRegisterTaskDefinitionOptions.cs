using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "register-task-definition")]
public record AwsEcsRegisterTaskDefinitionOptions(
[property: CliOption("--family")] string Family,
[property: CliOption("--container-definitions")] string[] ContainerDefinitions
) : AwsOptions
{
    [CliOption("--task-role-arn")]
    public string? TaskRoleArn { get; set; }

    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--network-mode")]
    public string? NetworkMode { get; set; }

    [CliOption("--volumes")]
    public string[]? Volumes { get; set; }

    [CliOption("--placement-constraints")]
    public string[]? PlacementConstraints { get; set; }

    [CliOption("--requires-compatibilities")]
    public string[]? RequiresCompatibilities { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--pid-mode")]
    public string? PidMode { get; set; }

    [CliOption("--ipc-mode")]
    public string? IpcMode { get; set; }

    [CliOption("--proxy-configuration")]
    public string? ProxyConfiguration { get; set; }

    [CliOption("--inference-accelerators")]
    public string[]? InferenceAccelerators { get; set; }

    [CliOption("--ephemeral-storage")]
    public string? EphemeralStorage { get; set; }

    [CliOption("--runtime-platform")]
    public string? RuntimePlatform { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}