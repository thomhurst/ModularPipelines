using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

public record HelmRegistryLoginOptions : HelmOptions
{
    [CommandLongSwitch("ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandLongSwitch("cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

    [BooleanCommandSwitch("help")]
    public bool? Help { get; set; }

    [BooleanCommandSwitch("insecure")]
    public bool? Insecure { get; set; }

    [CommandLongSwitch("key-file", SwitchValueSeparator = " ")]
    public string? KeyFile { get; set; }

    [CommandLongSwitch("password", SwitchValueSeparator = " ")]
    public string? Password { get; set; }

    [CommandLongSwitch("password-stdin", SwitchValueSeparator = " ")]
    public string? PasswordStdin { get; set; }

    [CommandLongSwitch("username", SwitchValueSeparator = " ")]
    public string? Username { get; set; }

}
