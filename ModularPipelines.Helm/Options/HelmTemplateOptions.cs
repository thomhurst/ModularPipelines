using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("template")]
public record HelmTemplateOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--api-versions", SwitchValueSeparator = " ")]
    public string[]? ApiVersions { get; set; }

    [BooleanCommandSwitch("--atomic")]
    public bool? Atomic { get; set; }

    [CommandEqualsSeparatorSwitch("--ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandEqualsSeparatorSwitch("--cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

    [BooleanCommandSwitch("--create-namespace")]
    public bool? CreateNamespace { get; set; }

    [BooleanCommandSwitch("--dependency-update")]
    public bool? DependencyUpdate { get; set; }

    [CommandEqualsSeparatorSwitch("--description", SwitchValueSeparator = " ")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--devel")]
    public bool? Devel { get; set; }

    [BooleanCommandSwitch("--disable-openapi-validation")]
    public bool? DisableOpenapiValidation { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--enable-dns")]
    public bool? EnableDns { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--generate-name")]
    public bool? GenerateName { get; set; }

    [BooleanCommandSwitch("--include-crds")]
    public bool? IncludeCrds { get; set; }

    [BooleanCommandSwitch("--insecure-skip-tls-verify")]
    public bool? InsecureSkipTlsVerify { get; set; }

    [BooleanCommandSwitch("--is-upgrade")]
    public bool? IsUpgrade { get; set; }

    [CommandEqualsSeparatorSwitch("--key-file", SwitchValueSeparator = " ")]
    public string? KeyFile { get; set; }

    [CommandEqualsSeparatorSwitch("--keyring", SwitchValueSeparator = " ")]
    public string? Keyring { get; set; }

    [CommandEqualsSeparatorSwitch("--kube-version", SwitchValueSeparator = " ")]
    public string? KubeVersion { get; set; }

    [CommandEqualsSeparatorSwitch("--name-template", SwitchValueSeparator = " ")]
    public string? NameTemplate { get; set; }

    [BooleanCommandSwitch("--no-hooks")]
    public bool? NoHooks { get; set; }

    [CommandEqualsSeparatorSwitch("--output-dir", SwitchValueSeparator = " ")]
    public string? OutputDir { get; set; }

    [BooleanCommandSwitch("--pass-credentials")]
    public bool? PassCredentials { get; set; }

    [CommandEqualsSeparatorSwitch("--password", SwitchValueSeparator = " ")]
    public string? Password { get; set; }

    [CommandEqualsSeparatorSwitch("--post-renderer", SwitchValueSeparator = " ")]
    public string? PostRenderer { get; set; }

    [CommandEqualsSeparatorSwitch("--post-renderer-args", SwitchValueSeparator = " ")]
    public string? PostRendererArgs { get; set; }

    [BooleanCommandSwitch("--release-name")]
    public bool? ReleaseName { get; set; }

    [BooleanCommandSwitch("--render-subchart-notes")]
    public bool? RenderSubchartNotes { get; set; }

    [BooleanCommandSwitch("--replace")]
    public bool? Replace { get; set; }

    [CommandEqualsSeparatorSwitch("--repo", SwitchValueSeparator = " ")]
    public string? Repo { get; set; }

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

    [CommandEqualsSeparatorSwitch("--show-only", SwitchValueSeparator = " ")]
    public string[]? ShowOnly { get; set; }

    [BooleanCommandSwitch("--skip-crds")]
    public bool? SkipCrds { get; set; }

    [BooleanCommandSwitch("--skip-tests")]
    public bool? SkipTests { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [CommandEqualsSeparatorSwitch("--username", SwitchValueSeparator = " ")]
    public string? Username { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [CommandEqualsSeparatorSwitch("--values", SwitchValueSeparator = " ")]
    public string[]? Values { get; set; }

    [BooleanCommandSwitch("--verify")]
    public bool? Verify { get; set; }

    [CommandEqualsSeparatorSwitch("--version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }

    [BooleanCommandSwitch("--wait-for-jobs")]
    public bool? WaitForJobs { get; set; }

}
