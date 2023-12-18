using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("extension", "add")]
public record AzExtensionAddOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--pip-extra-index-urls")]
    public string? PipExtraIndexUrls { get; set; }

    [CommandSwitch("--pip-proxy")]
    public string? PipProxy { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--system")]
    public string? System { get; set; }

    [CommandSwitch("--upgrade")]
    public string? Upgrade { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--yes")]
    public bool? Yes { get; set; } = true;
}

