using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("extension", "remove")]
public record AzExtensionRemoveOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--pip-extra-index-urls")]
    public string? PipExtraIndexUrls { get; set; }

    [CommandSwitch("--pip-proxy")]
    public string? PipProxy { get; set; }
}