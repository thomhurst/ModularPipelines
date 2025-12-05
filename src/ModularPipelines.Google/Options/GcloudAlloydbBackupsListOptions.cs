using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "backups", "list")]
public record GcloudAlloydbBackupsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}