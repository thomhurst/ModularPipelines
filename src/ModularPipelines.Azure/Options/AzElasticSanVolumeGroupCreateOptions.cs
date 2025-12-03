using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastic-san", "volume-group", "create")]
public record AzElasticSanVolumeGroupCreateOptions(
[property: CliOption("--elastic-san")] string ElasticSan,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--encryption")]
    public string? Encryption { get; set; }

    [CliOption("--encryption-properties")]
    public string? EncryptionProperties { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--network-acls")]
    public string? NetworkAcls { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protocol-type")]
    public string? ProtocolType { get; set; }
}