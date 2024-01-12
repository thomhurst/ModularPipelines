using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "list")]
public record GcloudKmsKeysListOptions(
[property: CommandSwitch("--keyring")] string Keyring,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;