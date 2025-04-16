using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "quickview")]
[ExcludeFromCodeCoverage]
public record DockerScoutQuickviewOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ImageOrDirectoryOrArchive { get; set; }

    [CommandSwitch("--env")]
    public virtual string? Env { get; set; }

    [CommandSwitch("--latest")]
    public virtual string? Latest { get; set; }

    [CommandSwitch("--org")]
    public virtual string? Org { get; set; }

    [CommandSwitch("--output")]
    public virtual string? Output { get; set; }

    [CommandSwitch("--platform")]
    public virtual string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public virtual string? Ref { get; set; }

    [CommandSwitch("--stream")]
    public virtual string? Stream { get; set; }
}
