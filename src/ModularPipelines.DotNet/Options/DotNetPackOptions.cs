using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("pack", "[<PROJECT>|<SOLUTION>]")]
[ExcludeFromCodeCoverage]
public record DotNetPackOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "[<PROJECT>|<SOLUTION>]")]
    public string? ProjectSolution { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--include-source")]
    public bool? IncludeSource { get; set; }

    [BooleanCommandSwitch("--include-symbols")]
    public bool? IncludeSymbols { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--no-dependencies")]
    public bool? NoDependencies { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; set; }

    [BooleanCommandSwitch("--nologo")]
    public bool? Nologo { get; set; }

    [CommandSwitch("--output")]
    public string? OutputDirectory { get; set; }

    [CommandSwitch("--runtime")]
    public string? RuntimeIdentifier { get; set; }

    [BooleanCommandSwitch("--serviceable")]
    public bool? Serviceable { get; set; }

    [BooleanCommandSwitch("--tl")]
    public bool? Tl { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }

    [CommandSwitch("--version-suffix")]
    public string? VersionSuffix { get; set; }
}
