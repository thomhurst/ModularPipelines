using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "nfs-shares", "create")]
public record GcloudBmsNfsSharesCreateOptions(
[property: PositionalArgument] string NfsShare,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--allowed-client")] string[] AllowedClient,
[property: CommandSwitch("--size-gib")] string SizeGib,
[property: CommandSwitch("--storage-type")] string StorageType
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}