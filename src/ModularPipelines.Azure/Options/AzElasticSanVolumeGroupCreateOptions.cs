using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic-san", "volume-group", "create")]
public record AzElasticSanVolumeGroupCreateOptions(
[property: CommandSwitch("--elastic-san")] string ElasticSan,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--encryption")]
    public string? Encryption { get; set; }

    [CommandSwitch("--encryption-properties")]
    public string? EncryptionProperties { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--network-acls")]
    public string? NetworkAcls { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protocol-type")]
    public string? ProtocolType { get; set; }
}

