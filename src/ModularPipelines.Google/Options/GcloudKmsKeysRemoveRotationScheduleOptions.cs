using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "remove-rotation-schedule")]
public record GcloudKmsKeysRemoveRotationScheduleOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Keyring,
[property: CliArgument] string Location
) : GcloudOptions;