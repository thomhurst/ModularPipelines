using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("download")]
public record DownloadOptions(
    [property: PositionalArgument] string Pkg
) : ChocoOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public bool? Prerelease { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--cert")]
    public string? Cert { get; set; }

    [CommandSwitch("--certpassword")]
    public string? Certpassword { get; set; }

    [CommandSwitch("--output-directory")]
    public string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--ignore-dependencies")]
    public bool? IgnoreDependencies { get; set; }

    [BooleanCommandSwitch("--installed-packages")]
    public bool? InstalledPackages { get; set; }

    [BooleanCommandSwitch("--ignore-unfound-packages")]
    public bool? IgnoreUnfoundPackages { get; set; }

    [BooleanCommandSwitch("--disable-package-repository-optimizations")]
    public bool? DisablePackageRepositoryOptimizations { get; set; }

    [BooleanCommandSwitch("--internalize")]
    public bool? Internalize { get; set; }

    [CommandSwitch("--resources-location")]
    public string? ResourcesLocation { get; set; }

    [CommandSwitch("--download-location")]
    public string? DownloadLocation { get; set; }

    [BooleanCommandSwitch("--internalize-all-urls")]
    public bool? InternalizeAllUrls { get; set; }

    [BooleanCommandSwitch("--append-use-original-location")]
    public bool? AppendUseOriginalLocation { get; set; }

    [BooleanCommandSwitch("--skip-download-cache")]
    public bool? SkipDownloadCache { get; set; }

    [BooleanCommandSwitch("--use-download-cache")]
    public bool? UseDownloadCache { get; set; }

    [BooleanCommandSwitch("--skip-virus-check")]
    public bool? SkipVirusCheck { get; set; }

    [BooleanCommandSwitch("--virus-check")]
    public bool? VirusCheck { get; set; }

    [CommandSwitch("--virus-positives-minimum")]
    public string? VirusPositivesMinimum { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public bool? ForceSelfService { get; set; }
}