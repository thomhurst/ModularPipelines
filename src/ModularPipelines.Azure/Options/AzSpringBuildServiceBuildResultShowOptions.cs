using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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