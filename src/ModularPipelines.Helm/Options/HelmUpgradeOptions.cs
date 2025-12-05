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
    public virtual string? CaFile { get; set; }

    [CliOption("--cert-file")]
    public virtual string? CertFile { get; set; }

    [CliFlag("--cleanup-on-fail")]
    public virtual bool? CleanupOnFail { get; set; }

    [CliFlag("--create-namespace")]
    public virtual bool? CreateNamespace { get; set; }

    [CliFlag("--dependency-update")]
    public virtual bool? DependencyUpdate { get; set; }

    [CliOption("--description")]
    public virtual string? Description { get; set; }

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
    public virtual int? HistoryMax { get; set; }

    [CliFlag("--insecure-skip-tls-verify")]
    public virtual bool? InsecureSkipTlsVerify { get; set; }

    [CliFlag("--install")]
    public virtual bool? Install { get; set; }

    [CliOption("--key-file")]
    public virtual string? KeyFile { get; set; }

    [CliOption("--keyring")]
    public virtual string? Keyring { get; set; }

    [CliFlag("--no-hooks")]
    public virtual bool? NoHooks { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--pass-credentials")]
    public virtual bool? PassCredentials { get; set; }

    [CliOption("--password")]
    public virtual string? Password { get; set; }

    [CliOption("--post-renderer")]
    public virtual string? PostRenderer { get; set; }

    [CliOption("--post-renderer-args")]
    public virtual string? PostRendererArgs { get; set; }

    [CliFlag("--render-subchart-notes")]
    public virtual bool? RenderSubchartNotes { get; set; }

    [CliOption("--repo")]
    public virtual string? Repo { get; set; }

    [CliFlag("--reset-values")]
    public virtual bool? ResetValues { get; set; }

    [CliFlag("--reuse-values")]
    public virtual bool? ReuseValues { get; set; }

    [CliOption("--set")]
    public virtual string[]? Set { get; set; }

    [CliOption("--set-file")]
    public virtual string[]? SetFile { get; set; }

    [CliOption("--set-json")]
    public virtual string[]? SetJson { get; set; }

    [CliOption("--set-literal")]
    public virtual string[]? SetLiteral { get; set; }

    [CliOption("--set-string")]
    public virtual string[]? SetString { get; set; }

    [CliFlag("--skip-crds")]
    public virtual bool? SkipCrds { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }

    [CliOption("--username")]
    public virtual string? Username { get; set; }

    [CliOption("--values")]
    public virtual string[]? Values { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    [CliFlag("--wait-for-jobs")]
    public virtual bool? WaitForJobs { get; set; }
}