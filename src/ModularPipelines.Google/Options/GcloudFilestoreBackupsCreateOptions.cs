using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "backups", "create")]
public record GcloudFilestoreBackupsCreateOptions(
[property: PositionalArgument] string Backup,
[property: CommandSwitch("--file-share")] string FileShare,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--instance-location")] string InstanceLocation,
[property: CommandSwitch("--instance-zone")] string InstanceZone
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}