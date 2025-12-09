using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("download")]
public record DownloadOptions(
    [property: CliArgument] string Pkg
) : ChocoOptions
{
    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }

    [CliFlag("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--password")]
    public virtual string? Password { get; set; }

    [CliOption("--cert")]
    public virtual string? Cert { get; set; }

    [CliOption("--certpassword")]
    public virtual string? Certpassword { get; set; }

    [CliOption("--output-directory")]
    public virtual string? OutputDirectory { get; set; }

    [CliFlag("--ignore-dependencies")]
    public virtual bool? IgnoreDependencies { get; set; }

    [CliFlag("--installed-packages")]
    public virtual bool? InstalledPackages { get; set; }

    [CliFlag("--ignore-unfound-packages")]
    public virtual bool? IgnoreUnfoundPackages { get; set; }

    [CliFlag("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [CliFlag("--internalize")]
    public virtual bool? Internalize { get; set; }

    [CliOption("--resources-location")]
    public virtual string? ResourcesLocation { get; set; }

    [CliOption("--download-location")]
    public virtual string? DownloadLocation { get; set; }

    [CliFlag("--internalize-all-urls")]
    public virtual bool? InternalizeAllUrls { get; set; }

    [CliFlag("--append-use-original-location")]
    public virtual bool? AppendUseOriginalLocation { get; set; }

    [CliFlag("--skip-download-cache")]
    public virtual bool? SkipDownloadCache { get; set; }

    [CliFlag("--use-download-cache")]
    public virtual bool? UseDownloadCache { get; set; }

    [CliFlag("--skip-virus-check")]
    public virtual bool? SkipVirusCheck { get; set; }

    [CliFlag("--virus-check")]
    public virtual bool? VirusCheck { get; set; }

    [CliOption("--virus-positives-minimum")]
    public virtual string? VirusPositivesMinimum { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}