using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("extension", "update")]
public record AzExtensionUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--pip-extra-index-urls")]
    public string? PipExtraIndexUrls { get; set; }

    [CommandSwitch("--pip-proxy")]
    public string? PipProxy { get; set; }
}