using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "backups", "list")]
public record GcloudActiveDirectoryDomainsBackupsListOptions(
[property: CommandSwitch("--domain")] string Domain
) : GcloudOptions;