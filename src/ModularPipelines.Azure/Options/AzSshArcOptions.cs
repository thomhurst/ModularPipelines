using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssh", "arc")]
public record AzSshArcOptions : AzOptions
{
    [CommandSwitch("--certificate-file")]
    public string? CertificateFile { get; set; }

    [CommandSwitch("--local-user")]
    public string? LocalUser { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CommandSwitch("--public-key-file")]
    public string? PublicKeyFile { get; set; }

    [BooleanCommandSwitch("--rdp")]
    public bool? Rdp { get; set; }

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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SshArgs { get; set; }
}