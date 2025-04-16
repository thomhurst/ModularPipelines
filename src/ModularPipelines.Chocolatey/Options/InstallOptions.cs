using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("install")]
public record InstallOptions(
    [property: PositionalArgument] string Pkg
) : ChocoOptions
{
    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [CommandSwitch("--version")]
    public virtual string? Version { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [BooleanCommandSwitch("--forcex86")]
    public virtual bool? Forcex86 { get; set; }

    [CommandSwitch("--install-arguments")]
    public virtual string? InstallArguments { get; set; }

    [BooleanCommandSwitch("--override-arguments")]
    public virtual bool? OverrideArguments { get; set; }

    [BooleanCommandSwitch("--not-silent")]
    public virtual bool? NotSilent { get; set; }

    [CommandSwitch("--package-parameters")]
    public virtual string? PackageParameters { get; set; }

    [BooleanCommandSwitch("--apply-install-arguments-to-dependencies")]
    public virtual bool? ApplyInstallArgumentsToDependencies { get; set; }

    [BooleanCommandSwitch("--apply-package-parameters-to-dependencies")]
    public virtual bool? ApplyPackageParametersToDependencies { get; set; }

    [BooleanCommandSwitch("--allow-downgrade")]
    public virtual bool? AllowDowngrade { get; set; }

    [BooleanCommandSwitch("--ignore-dependencies")]
    public virtual bool? IgnoreDependencies { get; set; }

    [BooleanCommandSwitch("--force-dependencies")]
    public virtual bool? ForceDependencies { get; set; }

    [BooleanCommandSwitch("--skip-automation-scripts")]
    public virtual bool? SkipAutomationScripts { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--password")]
    public virtual string? Password { get; set; }

    [CommandSwitch("--cert")]
    public virtual string? Cert { get; set; }

    [CommandSwitch("--certpassword")]
    public virtual string? Certpassword { get; set; }

    [BooleanCommandSwitch("--ignore-checksums")]
    public virtual bool? IgnoreChecksums { get; set; }

    [BooleanCommandSwitch("--allow-empty-checksums")]
    public virtual bool? AllowEmptyChecksums { get; set; }

    [BooleanCommandSwitch("--allow-empty-checksums-secure")]
    public virtual bool? AllowEmptyChecksumsSecure { get; set; }

    [BooleanCommandSwitch("--require-checksums")]
    public virtual bool? RequireChecksums { get; set; }

    [CommandSwitch("--download-checksum")]
    public virtual string? DownloadChecksum { get; set; }

    [CommandSwitch("--download-checksum-x64")]
    public virtual string? DownloadChecksumX64 { get; set; }

    [CommandSwitch("--download-checksum-type")]
    public virtual string? DownloadChecksumType { get; set; }

    [CommandSwitch("--download-checksum-type-x64")]
    public virtual string? DownloadChecksumTypeX64 { get; set; }

    [BooleanCommandSwitch("--ignore-package-exit-codes")]
    public virtual bool? IgnorePackageExitCodes { get; set; }

    [BooleanCommandSwitch("--use-package-exit-codes")]
    public virtual bool? UsePackageExitCodes { get; set; }

    [BooleanCommandSwitch("--stop-on-first-package-failure")]
    public virtual bool? StopOnFirstPackageFailure { get; set; }

    [BooleanCommandSwitch("--exit-when-reboot-detected")]
    public virtual bool? ExitWhenRebootDetected { get; set; }

    [BooleanCommandSwitch("--ignore-detected-reboot")]
    public virtual bool? IgnoreDetectedReboot { get; set; }

    [BooleanCommandSwitch("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [BooleanCommandSwitch("--pin-package")]
    public virtual bool? PinPackage { get; set; }

    [BooleanCommandSwitch("--skip-hooks")]
    public virtual bool? SkipHooks { get; set; }

    [BooleanCommandSwitch("--skip-download-cache")]
    public virtual bool? SkipDownloadCache { get; set; }

    [BooleanCommandSwitch("--use-download-cache")]
    public virtual bool? UseDownloadCache { get; set; }

    [BooleanCommandSwitch("--skip-virus-check")]
    public virtual bool? SkipVirusCheck { get; set; }

    [BooleanCommandSwitch("--virus-check")]
    public virtual bool? VirusCheck { get; set; }

    [CommandSwitch("--virus-positives-minimum")]
    public virtual string? VirusPositivesMinimum { get; set; }

    [CommandSwitch("--install-arguments-sensitive")]
    public virtual string? InstallArgumentsSensitive { get; set; }

    [CommandSwitch("--package-parameters-sensitive")]
    public virtual string? PackageParametersSensitive { get; set; }

    [CommandSwitch("--install-directory")]
    public virtual string? InstallDirectory { get; set; }

    [CommandSwitch("--maximum-download-bits-per-second")]
    public virtual string? MaximumDownloadBitsPerSecond { get; set; }

    [BooleanCommandSwitch("--deflate-package-size")]
    public virtual bool? DeflatePackageSize { get; set; }

    [BooleanCommandSwitch("--no-deflate-package-size")]
    public virtual bool? NoDeflatePackageSize { get; set; }

    [BooleanCommandSwitch("--deflate-nupkg-only")]
    public virtual bool? DeflateNupkgOnly { get; set; }

    [CommandSwitch("--note")]
    public virtual string? Note { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}