using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "update")]
public record GcloudKmsKeysUpdateOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Keyring,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--default-algorithm")]
    public string? DefaultAlgorithm { get; set; }

    [CliOption("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CliOption("--primary-version")]
    public string? PrimaryVersion { get; set; }

    [CliFlag("--remove-rotation-schedule")]
    public bool? RemoveRotationSchedule { get; set; }

    [CliOption("--rotation-period")]
    public string? RotationPeriod { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}