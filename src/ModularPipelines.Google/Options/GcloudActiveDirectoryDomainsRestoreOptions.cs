using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "restore")]
public record GcloudActiveDirectoryDomainsRestoreOptions(
[property: CliArgument] string Domain,
[property: CliOption("--backup")] string Backup
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}