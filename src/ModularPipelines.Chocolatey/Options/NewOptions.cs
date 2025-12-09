using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("new")]
public record NewOptions(
    [property: CliArgument] string Name
) : ChocoOptions
{
    [CliFlag("--automaticpackage")]
    public virtual bool? Automaticpackage { get; set; }

    [CliOption("--template-name")]
    public virtual string? TemplateName { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }

    [CliOption("--maintainer")]
    public virtual string? Maintainer { get; set; }

    [CliOption("--output-directory")]
    public virtual string? OutputDirectory { get; set; }

    [CliFlag("--use-built-in-template")]
    public virtual bool? UseBuiltInTemplate { get; set; }

    [CliOption("--url")]
    public virtual string? Url { get; set; }

    [CliOption("--url64")]
    public virtual string? Url64 { get; set; }

    [CliFlag("--use-original-files-location")]
    public virtual bool? UseOriginalFilesLocation { get; set; }

    [CliOption("--download-checksum")]
    public virtual string? DownloadChecksum { get; set; }

    [CliOption("--download-checksum-x64")]
    public virtual string? DownloadChecksumX64 { get; set; }

    [CliOption("--download-checksum-type")]
    public virtual string? DownloadChecksumType { get; set; }

    [CliFlag("--pause-on-error")]
    public virtual bool? PauseOnError { get; set; }

    [CliFlag("--build-packages")]
    public virtual bool? BuildPackages { get; set; }

    [CliFlag("--from-programs-and-features")]
    public virtual bool? FromProgramsAndFeatures { get; set; }

    [CliFlag("--remove-architecture-from-name")]
    public virtual bool? RemoveArchitectureFromName { get; set; }

    [CliFlag("--include-architecture-in-name")]
    public virtual bool? IncludeArchitectureInName { get; set; }
}