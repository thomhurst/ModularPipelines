using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("upgrade")]
public record UpgradeOptions(
    [property: PositionalArgument] string Pkg
) : ChocoOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public bool? Prerelease { get; set; }

    [BooleanCommandSwitch("--forcex86")]
    public bool? Forcex86 { get; set; }

    [CommandSwitch("--install-arguments")]
    public string? InstallArguments { get; set; }

    [BooleanCommandSwitch("--override-arguments")]
    public bool? OverrideArguments { get; set; }

    [BooleanCommandSwitch("--not-silent")]
    public bool? NotSilent { get; set; }

    [CommandSwitch("--package-parameters")]
    public string? PackageParameters { get; set; }

    [BooleanCommandSwitch("--apply-install-arguments-to-dependencies")]
    public bool? ApplyInstallArgumentsToDependencies { get; set; }

    [BooleanCommandSwitch("--apply-package-parameters-to-dependencies")]
    public bool? ApplyPackageParametersToDependencies { get; set; }

    [BooleanCommandSwitch("--allow-downgrade")]
    public bool? AllowDowngrade { get; set; }

    [BooleanCommandSwitch("--ignore-dependencies")]
    public bool? IgnoreDependencies { get; set; }

    [BooleanCommandSwitch("--skip-automation-scripts")]
    public bool? SkipAutomationScripts { get; set; }

    [BooleanCommandSwitch("--fail-on-unfound")]
    public bool? FailOnUnfound { get; set; }

    [BooleanCommandSwitch("--ignore-unfound")]
    public bool? IgnoreUnfound { get; set; }

    [BooleanCommandSwitch("--fail-on-not-installed")]
    public bool? FailOnNotInstalled { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--cert")]
    public string? Cert { get; set; }

    [CommandSwitch("--certpassword")]
    public string? Certpassword { get; set; }

    [BooleanCommandSwitch("--ignore-checksums")]
    public bool? IgnoreChecksums { get; set; }

    [BooleanCommandSwitch("--allow-empty-checksums")]
    public bool? AllowEmptyChecksums { get; set; }

    [BooleanCommandSwitch("--allow-empty-checksums-secure")]
    public bool? AllowEmptyChecksumsSecure { get; set; }

    [BooleanCommandSwitch("--require-checksums")]
    public bool? RequireChecksums { get; set; }

    [CommandSwitch("--download-checksum")]
    public string? DownloadChecksum { get; set; }

    [CommandSwitch("--download-checksum-x64")]
    public string? DownloadChecksumX64 { get; set; }

    [CommandSwitch("--download-checksum-type")]
    public string? DownloadChecksumType { get; set; }

    [CommandSwitch("--download-checksum-type-x64")]
    public string? DownloadChecksumTypeX64 { get; set; }

    [BooleanCommandSwitch("--ignore-package-exit-codes")]
    public bool? IgnorePackageExitCodes { get; set; }

    [BooleanCommandSwitch("--use-package-exit-codes")]
    public bool? UsePackageExitCodes { get; set; }

    [CommandSwitch("--except")]
    public string? Except { get; set; }

    [BooleanCommandSwitch("--stop-on-first-package-failure")]
    public bool? StopOnFirstPackageFailure { get; set; }

    [BooleanCommandSwitch("--skip-when-not-installed")]
    public bool? SkipWhenNotInstalled { get; set; }

    [BooleanCommandSwitch("--install-if-not-installed")]
    public bool? InstallIfNotInstalled { get; set; }

    [BooleanCommandSwitch("--exclude-prereleases")]
    public bool? ExcludePrereleases { get; set; }

    [BooleanCommandSwitch("--use-remembered-options")]
    public bool? UseRememberedOptions { get; set; }

    [BooleanCommandSwitch("--ignore-remembered-options")]
    public bool? IgnoreRememberedOptions { get; set; }

    [BooleanCommandSwitch("--exit-when-reboot-detected")]
    public bool? ExitWhenRebootDetected { get; set; }

    [BooleanCommandSwitch("--ignore-detected-reboot")]
    public bool? IgnoreDetectedReboot { get; set; }

    [BooleanCommandSwitch("--disable-package-repository-optimizations")]
    public bool? DisablePackageRepositoryOptimizations { get; set; }

    [BooleanCommandSwitch("--pin-package")]
    public bool? PinPackage { get; set; }

    [BooleanCommandSwitch("--skip-hooks")]
    public bool? SkipHooks { get; set; }

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

    [CommandSwitch("--install-arguments-sensitive")]
    public string? InstallArgumentsSensitive { get; set; }

    [CommandSwitch("--package-parameters-sensitive")]
    public string? PackageParametersSensitive { get; set; }

    [CommandSwitch("--install-directory")]
    public string? InstallDirectory { get; set; }

    [CommandSwitch("--maximum-download-bits-per-second")]
    public string? MaximumDownloadBitsPerSecond { get; set; }

    [BooleanCommandSwitch("--deflate-package-size")]
    public bool? DeflatePackageSize { get; set; }

    [BooleanCommandSwitch("--no-deflate-package-size")]
    public bool? NoDeflatePackageSize { get; set; }

    [BooleanCommandSwitch("--deflate-nupkg-only")]
    public bool? DeflateNupkgOnly { get; set; }

    [BooleanCommandSwitch("--exclude-chocolatey-packages-during-upgrade-all")]
    public bool? ExcludeChocolateyPackagesDuringUpgradeAll { get; set; }

    [BooleanCommandSwitch("--include-chocolatey-packages-during-upgrade-all")]
    public bool? IncludeChocolateyPackagesDuringUpgradeAll { get; set; }

    [CommandSwitch("--note")]
    public string? Note { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public bool? ForceSelfService { get; set; }
}