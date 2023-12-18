using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("new")]
public record NewOptions(
    [property: PositionalArgument] string Name
) : ChocoOptions
{
    [BooleanCommandSwitch("--automaticpackage")]
    public bool? Automaticpackage { get; set; }

    [CommandSwitch("--template-name")]
    public string? TemplateName { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--maintainer")]
    public string? Maintainer { get; set; }

    [CommandSwitch("--output-directory")]
    public string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--use-built-in-template")]
    public bool? UseBuiltInTemplate { get; set; }

    [CommandSwitch("--url")]
    public string? Url { get; set; }

    [CommandSwitch("--url64")]
    public string? Url64 { get; set; }

    [BooleanCommandSwitch("--use-original-files-location")]
    public bool? UseOriginalFilesLocation { get; set; }

    [CommandSwitch("--download-checksum")]
    public string? DownloadChecksum { get; set; }

    [CommandSwitch("--download-checksum-x64")]
    public string? DownloadChecksumX64 { get; set; }

    [CommandSwitch("--download-checksum-type")]
    public string? DownloadChecksumType { get; set; }

    [BooleanCommandSwitch("--pause-on-error")]
    public bool? PauseOnError { get; set; }

    [BooleanCommandSwitch("--build-packages")]
    public bool? BuildPackages { get; set; }

    [BooleanCommandSwitch("--from-programs-and-features")]
    public bool? FromProgramsAndFeatures { get; set; }

    [BooleanCommandSwitch("--remove-architecture-from-name")]
    public bool? RemoveArchitectureFromName { get; set; }

    [BooleanCommandSwitch("--include-architecture-in-name")]
    public bool? IncludeArchitectureInName { get; set; }
}