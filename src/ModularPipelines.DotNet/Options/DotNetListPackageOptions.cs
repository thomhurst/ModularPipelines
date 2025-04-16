using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetListPackageOptions : DotNetOptions
{
    public DotNetListPackageOptions(
        string projectSolution
    )
    {
        CommandParts = ["list", "[<PROJECT>|<SOLUTION>]", "package"];

        ProjectSolution = projectSolution;
    }

    public DotNetListPackageOptions()
    {
        CommandParts = ["list", "[<PROJECT>|<SOLUTION>]", "package"];
    }

    [PositionalArgument(PlaceholderName = "[<PROJECT>|<SOLUTION>]")]
    public string? ProjectSolution { get; set; }

    [CommandSwitch("--config")]
    public virtual string? Config { get; set; }

    [BooleanCommandSwitch("--deprecated")]
    public virtual bool? Deprecated { get; set; }

    [CommandSwitch("--framework")]
    public virtual IEnumerable<string>? Framework { get; set; }

    [BooleanCommandSwitch("--highest-minor")]
    public virtual bool? HighestMinor { get; set; }

    [BooleanCommandSwitch("--highest-patch")]
    public virtual bool? HighestPatch { get; set; }

    [BooleanCommandSwitch("--include-prerelease")]
    public virtual bool? IncludePrerelease { get; set; }

    [BooleanCommandSwitch("--include-transitive")]
    public virtual bool? IncludeTransitive { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--outdated")]
    public virtual bool? Outdated { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [BooleanCommandSwitch("--vulnerable")]
    public virtual bool? Vulnerable { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [CommandSwitch("--output-version")]
    public virtual string? OutputVersion { get; set; }
}
