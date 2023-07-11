using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("registry", "login")]
public record HelmRegistryLoginOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandEqualsSeparatorSwitch("--cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

    [BooleanCommandSwitch("--insecure")]
    public bool? Insecure { get; set; }

    [CommandEqualsSeparatorSwitch("--key-file", SwitchValueSeparator = " ")]
    public string? KeyFile { get; set; }

    [CommandEqualsSeparatorSwitch("--password", SwitchValueSeparator = " ")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--password-stdin")]
    public bool? PasswordStdin { get; set; }

    [CommandEqualsSeparatorSwitch("--username", SwitchValueSeparator = " ")]
    public string? Username { get; set; }

}
