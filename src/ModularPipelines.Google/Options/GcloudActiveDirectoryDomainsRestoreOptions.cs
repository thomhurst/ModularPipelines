using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "restore")]
public record GcloudActiveDirectoryDomainsRestoreOptions(
[property: PositionalArgument] string Domain,
[property: CommandSwitch("--backup")] string Backup
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}