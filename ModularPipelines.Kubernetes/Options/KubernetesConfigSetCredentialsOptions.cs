using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "set-credentials")]
public record KubernetesConfigSetCredentialsOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--auth-provider", SwitchValueSeparator = " ")]
    public string? AuthProvider { get; set; }

    [CommandEqualsSeparatorSwitch("--auth-provider-arg", SwitchValueSeparator = " ")]
    public string[]? AuthProviderArg { get; set; }

    [BooleanCommandSwitch("--embed-certs")]
    public bool? EmbedCerts { get; set; }

    [CommandEqualsSeparatorSwitch("--exec-api-version", SwitchValueSeparator = " ")]
    public string? ExecApiVersion { get; set; }

    [CommandEqualsSeparatorSwitch("--exec-arg", SwitchValueSeparator = " ")]
    public string[]? ExecArg { get; set; }

    [CommandEqualsSeparatorSwitch("--exec-command", SwitchValueSeparator = " ")]
    public string? ExecCommand { get; set; }

    [CommandEqualsSeparatorSwitch("--exec-env", SwitchValueSeparator = " ")]
    public string[]? ExecEnv { get; set; }
}
