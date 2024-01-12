using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "backups", "delete")]
public record GcloudActiveDirectoryDomainsBackupsDeleteOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Domain
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}