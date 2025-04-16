using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetPackOptions : DotNetOptions
{
    public DotNetPackOptions(
        string projectSolution
    )
    {
        CommandParts = ["pack", "[<PROJECT>|<SOLUTION>]"];

        ProjectSolution = projectSolution;
    }

    public DotNetPackOptions()
    {
        CommandParts = ["pack", "[<PROJECT>|<SOLUTION>]"];
    }

    [PositionalArgument(PlaceholderName = "[<PROJECT>|<SOLUTION>]")]
    public string? ProjectSolution { get; set; }

    [CommandSwitch("--configuration")]
    public virtual string? Configuration { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--include-source")]
    public virtual bool? IncludeSource { get; set; }

    [BooleanCommandSwitch("--include-symbols")]
    public virtual bool? IncludeSymbols { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--no-dependencies")]
    public virtual bool? NoDependencies { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [BooleanCommandSwitch("--nologo")]
    public virtual bool? Nologo { get; set; }

    [CommandSwitch("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CommandSwitch("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [BooleanCommandSwitch("--serviceable")]
    public virtual bool? Serviceable { get; set; }

    [BooleanCommandSwitch("--tl")]
    public virtual bool? Tl { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CommandSwitch("--version-suffix")]
    public virtual string? VersionSuffix { get; set; }
}
