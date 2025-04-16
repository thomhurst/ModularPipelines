using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("upgrade")]
[ExcludeFromCodeCoverage]
public record HelmUpgradeOptions : HelmOptions
{
    [BooleanCommandSwitch("--atomic")]
    public virtual bool? Atomic { get; set; }

    [CommandEqualsSeparatorSwitch("--ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandEqualsSeparatorSwitch("--cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

    [BooleanCommandSwitch("--cleanup-on-fail")]
    public virtual bool? CleanupOnFail { get; set; }

    [BooleanCommandSwitch("--create-namespace")]
    public virtual bool? CreateNamespace { get; set; }

    [BooleanCommandSwitch("--dependency-update")]
    public virtual bool? DependencyUpdate { get; set; }

    [CommandEqualsSeparatorSwitch("--description", SwitchValueSeparator = " ")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--devel")]
    public virtual bool? Devel { get; set; }

    [BooleanCommandSwitch("--disable-openapi-validation")]
    public virtual bool? DisableOpenapiValidation { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--enable-dns")]
    public virtual bool? EnableDns { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandEqualsSeparatorSwitch("--history-max", SwitchValueSeparator = " ")]
    public int? HistoryMax { get; set; }

    [BooleanCommandSwitch("--insecure-skip-tls-verify")]
    public virtual bool? InsecureSkipTlsVerify { get; set; }

    [BooleanCommandSwitch("--install")]
    public virtual bool? Install { get; set; }

    [CommandEqualsSeparatorSwitch("--key-file", SwitchValueSeparator = " ")]
    public string? KeyFile { get; set; }

    [CommandEqualsSeparatorSwitch("--keyring", SwitchValueSeparator = " ")]
    public string? Keyring { get; set; }

    [BooleanCommandSwitch("--no-hooks")]
    public virtual bool? NoHooks { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--pass-credentials")]
    public virtual bool? PassCredentials { get; set; }

    [CommandEqualsSeparatorSwitch("--password", SwitchValueSeparator = " ")]
    public string? Password { get; set; }

    [CommandEqualsSeparatorSwitch("--post-renderer", SwitchValueSeparator = " ")]
    public string? PostRenderer { get; set; }

    [CommandEqualsSeparatorSwitch("--post-renderer-args", SwitchValueSeparator = " ")]
    public string? PostRendererArgs { get; set; }

    [BooleanCommandSwitch("--render-subchart-notes")]
    public virtual bool? RenderSubchartNotes { get; set; }

    [CommandEqualsSeparatorSwitch("--repo", SwitchValueSeparator = " ")]
    public string? Repo { get; set; }

    [BooleanCommandSwitch("--reset-values")]
    public virtual bool? ResetValues { get; set; }

    [BooleanCommandSwitch("--reuse-values")]
    public virtual bool? ReuseValues { get; set; }

    [CommandEqualsSeparatorSwitch("--set", SwitchValueSeparator = " ")]
    public string[]? Set { get; set; }

    [CommandEqualsSeparatorSwitch("--set-file", SwitchValueSeparator = " ")]
    public string[]? SetFile { get; set; }

    [CommandEqualsSeparatorSwitch("--set-json", SwitchValueSeparator = " ")]
    public string[]? SetJson { get; set; }

    [CommandEqualsSeparatorSwitch("--set-literal", SwitchValueSeparator = " ")]
    public string[]? SetLiteral { get; set; }

    [CommandEqualsSeparatorSwitch("--set-string", SwitchValueSeparator = " ")]
    public string[]? SetString { get; set; }

    [BooleanCommandSwitch("--skip-crds")]
    public virtual bool? SkipCrds { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [CommandEqualsSeparatorSwitch("--username", SwitchValueSeparator = " ")]
    public string? Username { get; set; }

    [CommandEqualsSeparatorSwitch("--values", SwitchValueSeparator = " ")]
    public string[]? Values { get; set; }

    [BooleanCommandSwitch("--verify")]
    public virtual bool? Verify { get; set; }

    [CommandEqualsSeparatorSwitch("--version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }

    [BooleanCommandSwitch("--wait")]
    public virtual bool? Wait { get; set; }

    [BooleanCommandSwitch("--wait-for-jobs")]
    public virtual bool? WaitForJobs { get; set; }
}