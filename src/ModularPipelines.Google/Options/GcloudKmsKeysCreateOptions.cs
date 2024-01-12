using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "create")]
public record GcloudKmsKeysCreateOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--purpose")] string Purpose
) : GcloudOptions
{
    [CommandSwitch("--crypto-key-backend")]
    public string? CryptoKeyBackend { get; set; }

    [CommandSwitch("--default-algorithm")]
    public string? DefaultAlgorithm { get; set; }

    [CommandSwitch("--destroy-scheduled-duration")]
    public string? DestroyScheduledDuration { get; set; }

    [BooleanCommandSwitch("--import-only")]
    public bool? ImportOnly { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CommandSwitch("--protection-level")]
    public string? ProtectionLevel { get; set; }

    [CommandSwitch("--rotation-period")]
    public string? RotationPeriod { get; set; }

    [BooleanCommandSwitch("--skip-initial-version-creation")]
    public bool? SkipInitialVersionCreation { get; set; }
}