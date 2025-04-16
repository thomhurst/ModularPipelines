using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest", "create")]
[ExcludeFromCodeCoverage]
public record DockerManifestCreateOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Manifest { get; set; }

    [CommandSwitch("--amend")]
    public virtual string? Amend { get; set; }

    [CommandSwitch("--insecure")]
    public virtual string? Insecure { get; set; }
}
