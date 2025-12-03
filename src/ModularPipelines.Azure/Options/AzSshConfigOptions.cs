using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssh", "config")]
public record AzSshConfigOptions(
[property: CliOption("--file")] string File
) : AzOptions
{
    [CliOption("--certificate-file")]
    public string? CertificateFile { get; set; }

    [CliOption("--ip")]
    public string? Ip { get; set; }

    [CliOption("--keys-dest-folder")]
    public string? KeysDestFolder { get; set; }

    [CliOption("--local-user")]
    public string? LocalUser { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--overwrite")]
    public bool? Overwrite { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliFlag("--prefer-private-ip")]
    public bool? PreferPrivateIp { get; set; }

    [CliOption("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CliOption("--public-key-file")]
    public string? PublicKeyFile { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--ssh-client-folder")]
    public string? SshClientFolder { get; set; }

    [CliOption("--ssh-proxy-folder")]
    public string? SshProxyFolder { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}