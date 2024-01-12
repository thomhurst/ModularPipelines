using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "remove-rotation-schedule")]
public record GcloudKmsKeysRemoveRotationScheduleOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location
) : GcloudOptions;