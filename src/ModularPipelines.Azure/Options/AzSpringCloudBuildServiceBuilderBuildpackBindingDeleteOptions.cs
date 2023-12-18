using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "build-service", "builder", "buildpack-binding", "delete")]
public record AzSpringCloudBuildServiceBuilderBuildpackBindingDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--builder-name")]
    public string? BuilderName { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}