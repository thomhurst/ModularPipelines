using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "backups", "list")]
public record GcloudFilestoreBackupsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}