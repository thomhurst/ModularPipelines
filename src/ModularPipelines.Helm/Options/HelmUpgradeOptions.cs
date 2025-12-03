using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("upgrade")]
[ExcludeFromCodeCoverage]
public record HelmUpgradeOptions : HelmOptions
{
    [CliFlag("--atomic")]
    public virtual bool? Atomic { get; set; }

    [CliOption("--ca-file")]
    public string? CaFile { get; set; }

    [CliOption("--cert-file")]
    public string? CertFile { get; set; }

    [CliFlag("--cleanup-on-fail")]
    public virtual bool? CleanupOnFail { get; set; }

    [CliFlag("--create-namespace")]
    public virtual bool? CreateNamespace { get; set; }

    [CliFlag("--dependency-update")]
    public virtual bool? DependencyUpdate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--devel")]
    public virtual bool? Devel { get; set; }

    [CliFlag("--disable-openapi-validation")]
    public virtual bool? DisableOpenapiValidation { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--enable-dns")]
    public virtual bool? EnableDns { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--history-max")]
    public int? HistoryMax { get; set; }

    [CliFlag("--insecure-skip-tls-verify")]
    public virtual bool? InsecureSkipTlsVerify { get; set; }

    [CliFlag("--install")]
    public virtual bool? Install { get; set; }

    [CliOption("--key-file")]
    public string? KeyFile { get; set; }

    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliFlag("--no-hooks")]
    public virtual bool? NoHooks { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliFlag("--pass-credentials")]
    public virtual bool? PassCredentials { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--post-renderer")]
    public string? PostRenderer { get; set; }

    [CliOption("--post-renderer-args")]
    public string? PostRendererArgs { get; set; }

    [CliFlag("--render-subchart-notes")]
    public virtual bool? RenderSubchartNotes { get; set; }

    [CliOption("--repo")]
    public string? Repo { get; set; }

    [CliFlag("--reset-values")]
    public virtual bool? ResetValues { get; set; }

    [CliFlag("--reuse-values")]
    public virtual bool? ReuseValues { get; set; }

    [CliOption("--set")]
    public string[]? Set { get; set; }

    [CliOption("--set-file")]
    public string[]? SetFile { get; set; }

    [CliOption("--set-json")]
    public string[]? SetJson { get; set; }

    [CliOption("--set-literal")]
    public string[]? SetLiteral { get; set; }

    [CliOption("--set-string")]
    public string[]? SetString { get; set; }

    [CliFlag("--skip-crds")]
    public virtual bool? SkipCrds { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--values")]
    public string[]? Values { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    [CliFlag("--wait-for-jobs")]
    public virtual bool? WaitForJobs { get; set; }
}