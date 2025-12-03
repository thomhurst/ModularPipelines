using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "backups", "delete")]
public record GcloudAlloydbBackupsDeleteOptions(
[property: CliArgument] string Backup,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}