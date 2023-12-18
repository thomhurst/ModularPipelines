using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "app", "deployment", "create")]
public record AzSpringCloudAppDeploymentCreateOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--artifact-path")]
    public string? ArtifactPath { get; set; }

    [CommandSwitch("--build-env")]
    public string? BuildEnv { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

    [CommandSwitch("--config-file-patterns")]
    public string? ConfigFilePatterns { get; set; }

    [CommandSwitch("--container-args")]
    public string? ContainerArgs { get; set; }

    [CommandSwitch("--container-command")]
    public string? ContainerCommand { get; set; }

    [CommandSwitch("--container-image")]
    public string? ContainerImage { get; set; }

    [CommandSwitch("--container-registry")]
    public string? ContainerRegistry { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [BooleanCommandSwitch("--disable-probe")]
    public bool? DisableProbe { get; set; }

    [BooleanCommandSwitch("--disable-validation")]
    public bool? DisableValidation { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--jvm-options")]
    public string? JvmOptions { get; set; }

    [CommandSwitch("--main-entry")]
    public string? MainEntry { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [BooleanCommandSwitch("--skip-clone-settings")]
    public bool? SkipCloneSettings { get; set; }

    [CommandSwitch("--source-path")]
    public string? SourcePath { get; set; }

    [CommandSwitch("--target-module")]
    public string? TargetModule { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}

