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
    public virtual bool? Automaticpackage { get; set; }

    [CommandSwitch("--template-name")]
    public virtual string? TemplateName { get; set; }

    [CommandSwitch("--version")]
    public virtual string? Version { get; set; }

    [CommandSwitch("--maintainer")]
    public virtual string? Maintainer { get; set; }

    [CommandSwitch("--output-directory")]
    public virtual string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--use-built-in-template")]
    public virtual bool? UseBuiltInTemplate { get; set; }

    [CommandSwitch("--url")]
    public virtual string? Url { get; set; }

    [CommandSwitch("--url64")]
    public virtual string? Url64 { get; set; }

    [BooleanCommandSwitch("--use-original-files-location")]
    public virtual bool? UseOriginalFilesLocation { get; set; }

    [CommandSwitch("--download-checksum")]
    public virtual string? DownloadChecksum { get; set; }

    [CommandSwitch("--download-checksum-x64")]
    public virtual string? DownloadChecksumX64 { get; set; }

    [CommandSwitch("--download-checksum-type")]
    public virtual string? DownloadChecksumType { get; set; }

    [BooleanCommandSwitch("--pause-on-error")]
    public virtual bool? PauseOnError { get; set; }

    [BooleanCommandSwitch("--build-packages")]
    public virtual bool? BuildPackages { get; set; }

    [BooleanCommandSwitch("--from-programs-and-features")]
    public virtual bool? FromProgramsAndFeatures { get; set; }

    [BooleanCommandSwitch("--remove-architecture-from-name")]
    public virtual bool? RemoveArchitectureFromName { get; set; }

    [BooleanCommandSwitch("--include-architecture-in-name")]
    public virtual bool? IncludeArchitectureInName { get; set; }
}