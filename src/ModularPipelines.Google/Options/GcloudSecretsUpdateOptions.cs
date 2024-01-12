using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "update")]
public record GcloudSecretsUpdateOptions(
[property: PositionalArgument] string Secret
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--add-topics")]
    public string[]? AddTopics { get; set; }

    [BooleanCommandSwitch("--clear-topics")]
    public bool? ClearTopics { get; set; }

    [CommandSwitch("--remove-topics")]
    public string[]? RemoveTopics { get; set; }

    [BooleanCommandSwitch("--clear-annotations")]
    public bool? ClearAnnotations { get; set; }

    [CommandSwitch("--remove-annotations")]
    public string[]? RemoveAnnotations { get; set; }

    [CommandSwitch("--update-annotations")]
    public IEnumerable<KeyValue>? UpdateAnnotations { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--clear-version-aliases")]
    public bool? ClearVersionAliases { get; set; }

    [CommandSwitch("--remove-version-aliases")]
    public string[]? RemoveVersionAliases { get; set; }

    [CommandSwitch("--update-version-aliases")]
    public IEnumerable<KeyValue>? UpdateVersionAliases { get; set; }

    [CommandSwitch("--expire-time")]
    public string? ExpireTime { get; set; }

    [BooleanCommandSwitch("--remove-expiration")]
    public bool? RemoveExpiration { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }

    [CommandSwitch("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [BooleanCommandSwitch("--remove-next-rotation-time")]
    public bool? RemoveNextRotationTime { get; set; }

    [BooleanCommandSwitch("--remove-rotation-period")]
    public bool? RemoveRotationPeriod { get; set; }

    [BooleanCommandSwitch("--remove-rotation-schedule")]
    public bool? RemoveRotationSchedule { get; set; }

    [CommandSwitch("--rotation-period")]
    public string? RotationPeriod { get; set; }
}