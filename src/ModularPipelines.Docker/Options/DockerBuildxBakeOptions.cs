using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "bake")]
[ExcludeFromCodeCoverage]
public record DockerBuildxBakeOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Target { get; set; }

    [CommandSwitch("--file")]
    public virtual string? File { get; set; }

    [CommandSwitch("--load")]
    public virtual string? Load { get; set; }

    [CommandSwitch("--metadata-file")]
    public virtual string? MetadataFile { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CommandSwitch("--print")]
    public virtual string? Print { get; set; }

    [CommandSwitch("--progress")]
    public virtual string? Progress { get; set; }

    [CommandSwitch("--provenance")]
    public virtual string? Provenance { get; set; }

    [BooleanCommandSwitch("--pull")]
    public virtual bool? Pull { get; set; }

    [BooleanCommandSwitch("--push")]
    public virtual bool? Push { get; set; }

    [BooleanCommandSwitch("--sbom")]
    public virtual bool? Sbom { get; set; }

    [CommandSwitch("--set")]
    public virtual string? Set { get; set; }
}
