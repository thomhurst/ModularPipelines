using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetToolUpdateOptions : DotNetOptions
{
    public DotNetToolUpdateOptions()
    {
        CommandParts = ["tool", "update", "<PACKAGE_ID>"];
    }

    [CommandSwitch("--add-source")]
    public string? AddSource { get; set; }

    [CommandSwitch("--configfile")]
    public string? Configfile { get; set; }

    [BooleanCommandSwitch("--disable-parallel")]
    public bool? DisableParallel { get; set; }

    [CommandSwitch("--framework")]
    public string? Framework { get; set; }

    [BooleanCommandSwitch("--ignore-failed-sources")]
    public bool? IgnoreFailedSources { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public bool? Prerelease { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--tool-manifest")]
    public string? ToolManifest { get; set; }
}
