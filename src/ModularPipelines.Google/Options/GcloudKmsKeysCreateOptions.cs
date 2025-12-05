using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "create")]
public record GcloudKmsKeysCreateOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Keyring,
[property: CliArgument] string Location,
[property: CliOption("--purpose")] string Purpose
) : GcloudOptions
{
    [CliOption("--crypto-key-backend")]
    public string? CryptoKeyBackend { get; set; }

    [CliOption("--default-algorithm")]
    public string? DefaultAlgorithm { get; set; }

    [CliOption("--destroy-scheduled-duration")]
    public string? DestroyScheduledDuration { get; set; }

    [CliFlag("--import-only")]
    public bool? ImportOnly { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CliOption("--protection-level")]
    public string? ProtectionLevel { get; set; }

    [CliOption("--rotation-period")]
    public string? RotationPeriod { get; set; }

    [CliFlag("--skip-initial-version-creation")]
    public bool? SkipInitialVersionCreation { get; set; }
}