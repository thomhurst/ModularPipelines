using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("push")]
public record HelmPushOptions : HelmOptions
{
    [CommandLongSwitch("ca-file", SwitchValueSeparator = " ")]
    public string? CaFile { get; set; }

    [CommandLongSwitch("cert-file", SwitchValueSeparator = " ")]
    public string? CertFile { get; set; }

    [BooleanCommandSwitch("insecure-skip-tls-verify")]
    public bool? InsecureSkipTlsVerify { get; set; }

    [CommandLongSwitch("key-file", SwitchValueSeparator = " ")]
    public string? KeyFile { get; set; }

}
