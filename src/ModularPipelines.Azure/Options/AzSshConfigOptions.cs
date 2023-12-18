using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssh", "config")]
public record AzSshConfigOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [CommandSwitch("--certificate-file")]
    public string? CertificateFile { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--keys-dest-folder")]
    public string? KeysDestFolder { get; set; }

    [CommandSwitch("--local-user")]
    public string? LocalUser { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [BooleanCommandSwitch("--prefer-private-ip")]
    public bool? PreferPrivateIp { get; set; }

    [CommandSwitch("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CommandSwitch("--public-key-file")]
    public string? PublicKeyFile { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--ssh-client-folder")]
    public string? SshClientFolder { get; set; }

    [CommandSwitch("--ssh-proxy-folder")]
    public string? SshProxyFolder { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

