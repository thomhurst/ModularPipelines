using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "backups", "describe")]
public record GcloudActiveDirectoryDomainsBackupsDescribeOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Domain
) : GcloudOptions;