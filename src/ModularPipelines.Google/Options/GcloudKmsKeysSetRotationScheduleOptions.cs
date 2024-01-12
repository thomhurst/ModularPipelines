using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "set-rotation-schedule")]
public record GcloudKmsKeysSetRotationScheduleOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CommandSwitch("--rotation-period")]
    public string? RotationPeriod { get; set; }
}