using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

public record HelmUpgradeOptions : HelmOptions
{
    [BooleanCommandSwitch("atomic")]
    public bool? Atomic { get; set; }

    [CommandLongSwitch("ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandLongSwitch("cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

    [BooleanCommandSwitch("cleanup-on-fail")]
    public bool? CleanupOnFail { get; set; }

    [BooleanCommandSwitch("create-namespace")]
    public bool? CreateNamespace { get; set; }

    [BooleanCommandSwitch("dependency-update")]
    public bool? DependencyUpdate { get; set; }

    [CommandLongSwitch("description", SwitchValueSeparator = " ")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("devel")] public bool? Devel { get; set; }

    [BooleanCommandSwitch("disable-openapi-validation")]
    public bool? DisableOpenapiValidation { get; set; }

    [BooleanCommandSwitch("dry-run")] public bool? DryRun { get; set; }

    [BooleanCommandSwitch("enable-dns")] public bool? EnableDns { get; set; }

    [BooleanCommandSwitch("force")] public bool? Force { get; set; }

    [CommandLongSwitch("history-max", SwitchValueSeparator = " ")]
    public int? HistoryMax { get; set; }

    [BooleanCommandSwitch("insecure-skip-tls-verify")]
    public bool? InsecureSkipTlsVerify { get; set; }

    [BooleanCommandSwitch("install")]
    public bool? Install { get; set; }

    [CommandLongSwitch("key-file", SwitchValueSeparator = " ")]
    public string? KeyFile { get; set; }

    [CommandLongSwitch("keyring", SwitchValueSeparator = " ")]
    public string? Keyring { get; set; }

    [BooleanCommandSwitch("no-hooks")] public bool? NoHooks { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("pass-credentials")]
    public bool? PassCredentials { get; set; }

    [CommandLongSwitch("password", SwitchValueSeparator = " ")]
    public string? Password { get; set; }

    [CommandLongSwitch("post-renderer", SwitchValueSeparator = " ")]
    public string? PostRenderer { get; set; }

    [CommandLongSwitch("post-renderer-args", SwitchValueSeparator = " ")]
    public string? PostRendererArgs { get; set; }

    [BooleanCommandSwitch("render-subchart-notes")]
    public bool? RenderSubchartNotes { get; set; }

    [CommandLongSwitch("repo", SwitchValueSeparator = " ")]
    public string? Repo { get; set; }

    [BooleanCommandSwitch("reset-values")] public bool? ResetValues { get; set; }

    [BooleanCommandSwitch("reuse-values")] public bool? ReuseValues { get; set; }

    [CommandLongSwitch("set", SwitchValueSeparator = " ")]
    public string[]? Set { get; set; }

    [CommandLongSwitch("set-file", SwitchValueSeparator = " ")]
    public string[]? SetFile { get; set; }

    [CommandLongSwitch("set-json", SwitchValueSeparator = " ")]
    public string[]? SetJson { get; set; }

    [CommandLongSwitch("set-literal", SwitchValueSeparator = " ")]
    public string[]? SetLiteral { get; set; }

    [CommandLongSwitch("set-string", SwitchValueSeparator = " ")]
    public string[]? SetString { get; set; }

    [BooleanCommandSwitch("skip-crds")] public bool? SkipCrds { get; set; }

    [CommandLongSwitch("timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [CommandLongSwitch("username", SwitchValueSeparator = " ")]
    public string? Username { get; set; }

    [CommandSwitch("f")]
    public string[]? Values { get; set; }

    [CommandSwitch("f")]
    public string[]? Files { get; set; }

    [BooleanCommandSwitch("verify")]
    public bool? Verify { get; set; }

    [CommandLongSwitch("version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }

    [BooleanCommandSwitch("wait")]
    public bool? Wait { get; set; }

    [BooleanCommandSwitch("wait-for-jobs")]
    public bool? WaitForJobs { get; set; }
}
