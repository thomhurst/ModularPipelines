using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "app", "deployment", "start-jfr")]
public record AzSpringCloudAppDeploymentStartJfrOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--app-instance")] string AppInstance,
[property: CommandSwitch("--file-path")] string FilePath,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }
}

