using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("install")]
public record NpmInstallOptions : NpmOptions
{
    [BooleanCommandSwitch("--save")]
    public bool? Save { get; set; }

    [BooleanCommandSwitch("--save-exact")]
    public bool? SaveExact { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--install-strategy")]
    public string? InstallStrategy { get; set; }

    [CommandSwitch("--omit")]
    public string? Omit { get; set; }

    [CommandSwitch("--include")]
    public string? Include { get; set; }

    [BooleanCommandSwitch("--strict-peer-deps")]
    public bool? StrictPeerDeps { get; set; }

    [BooleanCommandSwitch("--prefer-dedupe")]
    public bool? PreferDedupe { get; set; }

    [BooleanCommandSwitch("--package-lock")]
    public bool? PackageLock { get; set; }

    [BooleanCommandSwitch("--package-lock-only")]
    public bool? PackageLockOnly { get; set; }

    [BooleanCommandSwitch("--foreground-scripts")]
    public bool? ForegroundScripts { get; set; }

    [BooleanCommandSwitch("--ignore-scripts")]
    public bool? IgnoreScripts { get; set; }

    [BooleanCommandSwitch("--audit")]
    public bool? Audit { get; set; }

    [BooleanCommandSwitch("--bin-links")]
    public bool? BinLinks { get; set; }

    [BooleanCommandSwitch("--fund")]
    public bool? Fund { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }

    [BooleanCommandSwitch("--install-links")]
    public bool? InstallLinks { get; set; }

    [BooleanCommandSwitch("--save-prod")]
    public bool? SaveProd { get; set; }

    [BooleanCommandSwitch("--save-dev")]
    public bool? SaveDev { get; set; }

    [BooleanCommandSwitch("--save-optional")]
    public bool? SaveOptional { get; set; }

    [BooleanCommandSwitch("--no-save")]
    public bool? NoSave { get; set; }

    [BooleanCommandSwitch("--save-bundle")]
    public bool? SaveBundle { get; set; }
}