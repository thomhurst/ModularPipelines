using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "backups", "create")]
public record GcloudFilestoreBackupsCreateOptions(
[property: CliArgument] string Backup,
[property: CliOption("--file-share")] string FileShare,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--region")] string Region,
[property: CliOption("--instance-location")] string InstanceLocation,
[property: CliOption("--instance-zone")] string InstanceZone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}