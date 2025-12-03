using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "backups", "delete")]
public record GcloudActiveDirectoryDomainsBackupsDeleteOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Domain
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}