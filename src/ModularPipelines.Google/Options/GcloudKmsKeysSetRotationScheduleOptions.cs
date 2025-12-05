using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "set-rotation-schedule")]
public record GcloudKmsKeysSetRotationScheduleOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Keyring,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CliOption("--rotation-period")]
    public string? RotationPeriod { get; set; }
}