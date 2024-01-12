using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "storage-pools", "create")]
public record GcloudNetappStoragePoolsCreateOptions(
[property: PositionalArgument] string StoragePool,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--capacity")] string Capacity,
[property: CommandSwitch("--network")] string[] Network,
[property: CommandSwitch("--service-level")] string ServiceLevel
) : GcloudOptions
{
    [CommandSwitch("--active-directory")]
    public string? ActiveDirectory { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--enable-ldap")]
    public string? EnableLdap { get; set; }

    [CommandSwitch("--kms-config")]
    public string? KmsConfig { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}