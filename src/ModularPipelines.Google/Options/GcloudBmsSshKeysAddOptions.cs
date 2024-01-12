using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "ssh-keys", "add")]
public record GcloudBmsSshKeysAddOptions(
[property: PositionalArgument] string SshKey,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--key-file")] string KeyFile
) : GcloudOptions;