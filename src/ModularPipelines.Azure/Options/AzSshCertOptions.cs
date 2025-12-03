using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssh", "cert")]
public record AzSshCertOptions : AzOptions
{
    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--public-key-file")]
    public string? PublicKeyFile { get; set; }

    [CliOption("--ssh-client-folder")]
    public string? SshClientFolder { get; set; }
}