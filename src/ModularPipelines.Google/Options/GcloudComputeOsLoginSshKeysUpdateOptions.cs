using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-login", "ssh-keys", "update")]
public record GcloudComputeOsLoginSshKeysUpdateOptions(
[property: CommandSwitch("--ttl")] string Ttl,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--key-file")] string KeyFile
) : GcloudOptions;