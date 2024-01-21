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
    public string? Config { get; set; }

    [BooleanCommandSwitch("--deprecated")]
    public bool? Deprecated { get; set; }

    [CommandSwitch("--framework")]
    public IEnumerable<string>? Framework { get; set; }

    [BooleanCommandSwitch("--highest-minor")]
    public bool? HighestMinor { get; set; }

    [BooleanCommandSwitch("--highest-patch")]
    public bool? HighestPatch { get; set; }

    [BooleanCommandSwitch("--include-prerelease")]
    public bool? IncludePrerelease { get; set; }

    [BooleanCommandSwitch("--include-transitive")]
    public bool? IncludeTransitive { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--outdated")]
    public bool? Outdated { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }

    [BooleanCommandSwitch("--vulnerable")]
    public bool? Vulnerable { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--output-version")]
    public string? OutputVersion { get; set; }
}
