using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "instance-configs", "create")]
public record GcloudSpannerInstanceConfigsCreateOptions(
[property: CliArgument] string InstanceConfig,
[property: CliOption("--base-config")] string BaseConfig,
[property: CliOption("--replicas")] string[] Replicas,
[property: CliFlag("location")] bool Location,
[property: CliFlag("type")] bool Type,
[property: CliFlag("READ_ONLY")] bool ReadOnly,
[property: CliFlag("READ_WRITE")] bool ReadWrite,
[property: CliFlag("WITNESS")] bool Witness,
[property: CliOption("--clone-config")] string CloneConfig,
[property: CliOption("--add-replicas")] string[] AddReplicas,
[property: CliOption("--skip-replicas")] string[] SkipReplicas
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}