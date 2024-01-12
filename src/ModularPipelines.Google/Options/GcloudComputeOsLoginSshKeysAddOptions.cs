using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-login", "ssh-keys", "add")]
public record GcloudComputeOsLoginSshKeysAddOptions(
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--key-file")] string KeyFile
) : GcloudOptions
{
    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }
}