using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-login", "ssh-keys", "add")]
public record GcloudComputeOsLoginSshKeysAddOptions(
[property: CliOption("--key")] string Key,
[property: CliOption("--key-file")] string KeyFile
) : GcloudOptions
{
    [CliOption("--ttl")]
    public string? Ttl { get; set; }
}