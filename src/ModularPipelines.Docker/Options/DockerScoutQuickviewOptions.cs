using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "quickview")]
[ExcludeFromCodeCoverage]
public record DockerScoutQuickviewOptions : DockerOptions
{
    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [CommandSwitch("--latest")]
    public string? Latest { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public string? Ref { get; set; }

    [CommandSwitch("--stream")]
    public string? Stream { get; set; }
}
