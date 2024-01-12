using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "instance-configs", "create")]
public record GcloudSpannerInstanceConfigsCreateOptions(
[property: PositionalArgument] string InstanceConfig,
[property: CommandSwitch("--base-config")] string BaseConfig,
[property: CommandSwitch("--replicas")] string[] Replicas,
[property: BooleanCommandSwitch("location")] bool Location,
[property: BooleanCommandSwitch("type")] bool Type,
[property: BooleanCommandSwitch("READ_ONLY")] bool ReadOnly,
[property: BooleanCommandSwitch("READ_WRITE")] bool ReadWrite,
[property: BooleanCommandSwitch("WITNESS")] bool Witness,
[property: CommandSwitch("--clone-config")] string CloneConfig,
[property: CommandSwitch("--add-replicas")] string[] AddReplicas,
[property: CommandSwitch("--skip-replicas")] string[] SkipReplicas
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}