using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetToolInstallOptions : DotNetOptions
{
    [CommandSwitch("--arch")]
    public string? Architecture { get; set; }

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

    [CommandSwitch("--tool-manifest")]
    public string? ToolManifest { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }

    [CommandSwitch("--version")]
    public string? VersionNumber { get; set; }

    [PositionalArgument(PlaceholderName = "<PACKAGE_NAME>")]
    public string? PackageName { get; set; }

    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [BooleanCommandSwitch("--create-manifest-if-needed")]
    public bool? CreateManifestIfNeeded { get; set; }
}
