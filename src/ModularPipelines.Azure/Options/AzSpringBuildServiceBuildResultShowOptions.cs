using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "build-service", "build", "result", "show")]
public record AzSpringBuildServiceBuildResultShowOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--build-name")]
    public string? BuildName { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }
}