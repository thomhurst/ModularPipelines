using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logicapp", "deployment", "source", "config-zip")]
public record AzLogicappDeploymentSourceConfigZipOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--src")] string Src
) : AzOptions
{
    [BooleanCommandSwitch("--build-remote")]
    public bool? BuildRemote { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}