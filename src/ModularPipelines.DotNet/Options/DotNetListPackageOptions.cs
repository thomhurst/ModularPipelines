using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("list", "[<PROJECT>|<SOLUTION>]", "package")]
[ExcludeFromCodeCoverage]
public record DotNetListPackageOptions : DotNetOptions
{
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
