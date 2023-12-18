using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic-san", "volume", "create")]
public record AzElasticSanVolumeCreateOptions(
[property: CommandSwitch("--elastic-san")] string ElasticSan,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--size-gib")] string SizeGib,
[property: CommandSwitch("--volume-group")] string VolumeGroup
) : AzOptions
{
    [CommandSwitch("--creation-data")]
    public string? CreationData { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

