using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "build-service", "builder", "buildpack-binding", "create")]
public record AzSpringBuildServiceBuilderBuildpackBindingCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--builder-name")]
    public string? BuilderName { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [CommandSwitch("--secrets")]
    public string? Secrets { get; set; }
}