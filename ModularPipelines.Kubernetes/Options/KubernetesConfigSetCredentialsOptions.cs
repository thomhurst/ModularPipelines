using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "set-credentials")]
public record KubernetesConfigSetCredentialsOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [CommandLongSwitch("auth-provider", SwitchValueSeparator = " ")]
    public string? AuthProvider { get; set; }

    [CommandLongSwitch("auth-provider-arg", SwitchValueSeparator = " ")]
    public string[]? AuthProviderArg { get; set; }

    [BooleanCommandSwitch("embed-certs")]
    public bool? EmbedCerts { get; set; }

    [CommandLongSwitch("exec-api-version", SwitchValueSeparator = " ")]
    public string? ExecApiVersion { get; set; }

    [CommandLongSwitch("exec-arg", SwitchValueSeparator = " ")]
    public string[]? ExecArg { get; set; }

    [CommandLongSwitch("exec-command", SwitchValueSeparator = " ")]
    public string? ExecCommand { get; set; }

    [CommandLongSwitch("exec-env", SwitchValueSeparator = " ")]
    public string[]? ExecEnv { get; set; }

}
