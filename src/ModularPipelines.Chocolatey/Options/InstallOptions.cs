using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("install")]
public record InstallOptions(
    [property: CliArgument] string Pkg
) : ChocoOptions
{
    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }

    [CliFlag("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CliFlag("--forcex86")]
    public virtual bool? Forcex86 { get; set; }

    [CliOption("--install-arguments")]
    public virtual string? InstallArguments { get; set; }

    [CliFlag("--override-arguments")]
    public virtual bool? OverrideArguments { get; set; }

    [CliFlag("--not-silent")]
    public virtual bool? NotSilent { get; set; }

    [CliOption("--package-parameters")]
    public virtual string? PackageParameters { get; set; }

    [CliFlag("--apply-install-arguments-to-dependencies")]
    public virtual bool? ApplyInstallArgumentsToDependencies { get; set; }

    [CliFlag("--apply-package-parameters-to-dependencies")]
    public virtual bool? ApplyPackageParametersToDependencies { get; set; }

    [CliFlag("--allow-downgrade")]
    public virtual bool? AllowDowngrade { get; set; }

    [CliFlag("--ignore-dependencies")]
    public virtual bool? IgnoreDependencies { get; set; }

    [CliFlag("--force-dependencies")]
    public virtual bool? ForceDependencies { get; set; }

    [CliFlag("--skip-automation-scripts")]
    public virtual bool? SkipAutomationScripts { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--password")]
    public virtual string? Password { get; set; }

    [CliOption("--cert")]
    public virtual string? Cert { get; set; }

    [CliOption("--certpassword")]
    public virtual string? Certpassword { get; set; }

    [CliFlag("--ignore-checksums")]
    public virtual bool? IgnoreChecksums { get; set; }

    [CliFlag("--allow-empty-checksums")]
    public virtual bool? AllowEmptyChecksums { get; set; }

    [CliFlag("--allow-empty-checksums-secure")]
    public virtual bool? AllowEmptyChecksumsSecure { get; set; }

    [CliFlag("--require-checksums")]
    public virtual bool? RequireChecksums { get; set; }

    [CliOption("--download-checksum")]
    public virtual string? DownloadChecksum { get; set; }

    [CliOption("--download-checksum-x64")]
    public virtual string? DownloadChecksumX64 { get; set; }

    [CliOption("--download-checksum-type")]
    public virtual string? DownloadChecksumType { get; set; }

    [CliOption("--download-checksum-type-x64")]
    public virtual string? DownloadChecksumTypeX64 { get; set; }

    [CliFlag("--ignore-package-exit-codes")]
    public virtual bool? IgnorePackageExitCodes { get; set; }

    [CliFlag("--use-package-exit-codes")]
    public virtual bool? UsePackageExitCodes { get; set; }

    [CliFlag("--stop-on-first-package-failure")]
    public virtual bool? StopOnFirstPackageFailure { get; set; }

    [CliFlag("--exit-when-reboot-detected")]
    public virtual bool? ExitWhenRebootDetected { get; set; }

    [CliFlag("--ignore-detected-reboot")]
    public virtual bool? IgnoreDetectedReboot { get; set; }

    [CliFlag("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [CliFlag("--pin-package")]
    public virtual bool? PinPackage { get; set; }

    [CliFlag("--skip-hooks")]
    public virtual bool? SkipHooks { get; set; }

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

    [CliOption("--install-arguments-sensitive")]
    public virtual string? InstallArgumentsSensitive { get; set; }

    [CliOption("--package-parameters-sensitive")]
    public virtual string? PackageParametersSensitive { get; set; }

    [CliOption("--install-directory")]
    public virtual string? InstallDirectory { get; set; }

    [CliOption("--maximum-download-bits-per-second")]
    public virtual string? MaximumDownloadBitsPerSecond { get; set; }

    [CliFlag("--deflate-package-size")]
    public virtual bool? DeflatePackageSize { get; set; }

    [CliFlag("--no-deflate-package-size")]
    public virtual bool? NoDeflatePackageSize { get; set; }

    [CliFlag("--deflate-nupkg-only")]
    public virtual bool? DeflateNupkgOnly { get; set; }

    [CliOption("--note")]
    public virtual string? Note { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}