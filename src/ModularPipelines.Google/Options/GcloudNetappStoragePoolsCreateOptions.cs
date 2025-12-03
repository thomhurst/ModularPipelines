using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "storage-pools", "create")]
public record GcloudNetappStoragePoolsCreateOptions(
[property: CliArgument] string StoragePool,
[property: CliArgument] string Location,
[property: CliOption("--capacity")] string Capacity,
[property: CliOption("--network")] string[] Network,
[property: CliOption("--service-level")] string ServiceLevel
) : GcloudOptions
{
    [CliOption("--active-directory")]
    public string? ActiveDirectory { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--enable-ldap")]
    public string? EnableLdap { get; set; }

    [CliOption("--kms-config")]
    public string? KmsConfig { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}