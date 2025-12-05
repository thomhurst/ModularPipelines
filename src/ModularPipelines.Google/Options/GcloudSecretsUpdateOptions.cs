using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "update")]
public record GcloudSecretsUpdateOptions(
[property: CliArgument] string Secret
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--add-topics")]
    public string[]? AddTopics { get; set; }

    [CliFlag("--clear-topics")]
    public bool? ClearTopics { get; set; }

    [CliOption("--remove-topics")]
    public string[]? RemoveTopics { get; set; }

    [CliFlag("--clear-annotations")]
    public bool? ClearAnnotations { get; set; }

    [CliOption("--remove-annotations")]
    public string[]? RemoveAnnotations { get; set; }

    [CliOption("--update-annotations")]
    public IEnumerable<KeyValue>? UpdateAnnotations { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliFlag("--clear-version-aliases")]
    public bool? ClearVersionAliases { get; set; }

    [CliOption("--remove-version-aliases")]
    public string[]? RemoveVersionAliases { get; set; }

    [CliOption("--update-version-aliases")]
    public IEnumerable<KeyValue>? UpdateVersionAliases { get; set; }

    [CliOption("--expire-time")]
    public string? ExpireTime { get; set; }

    [CliFlag("--remove-expiration")]
    public bool? RemoveExpiration { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }

    [CliOption("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CliFlag("--remove-next-rotation-time")]
    public bool? RemoveNextRotationTime { get; set; }

    [CliFlag("--remove-rotation-period")]
    public bool? RemoveRotationPeriod { get; set; }

    [CliFlag("--remove-rotation-schedule")]
    public bool? RemoveRotationSchedule { get; set; }

    [CliOption("--rotation-period")]
    public string? RotationPeriod { get; set; }
}