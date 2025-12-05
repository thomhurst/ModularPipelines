using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "nfs-shares", "create")]
public record GcloudBmsNfsSharesCreateOptions(
[property: CliArgument] string NfsShare,
[property: CliArgument] string Region,
[property: CliOption("--allowed-client")] string[] AllowedClient,
[property: CliOption("--size-gib")] string SizeGib,
[property: CliOption("--storage-type")] string StorageType
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}