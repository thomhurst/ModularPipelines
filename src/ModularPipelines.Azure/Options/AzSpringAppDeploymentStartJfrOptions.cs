using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "app", "deployment", "start-jfr")]
public record AzSpringAppDeploymentStartJfrOptions(
[property: CliOption("--app")] string App,
[property: CliOption("--app-instance")] string AppInstance,
[property: CliOption("--file-path")] string FilePath,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }
}