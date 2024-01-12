using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "ssh-keys", "remove")]
public record GcloudBmsSshKeysRemoveOptions(
[property: PositionalArgument] string SshKey
) : GcloudOptions;