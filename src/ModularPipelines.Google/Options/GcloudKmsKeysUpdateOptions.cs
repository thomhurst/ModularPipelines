using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "update")]
public record GcloudKmsKeysUpdateOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--default-algorithm")]
    public string? DefaultAlgorithm { get; set; }

    [CommandSwitch("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CommandSwitch("--primary-version")]
    public string? PrimaryVersion { get; set; }

    [BooleanCommandSwitch("--remove-rotation-schedule")]
    public bool? RemoveRotationSchedule { get; set; }

    [CommandSwitch("--rotation-period")]
    public string? RotationPeriod { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}