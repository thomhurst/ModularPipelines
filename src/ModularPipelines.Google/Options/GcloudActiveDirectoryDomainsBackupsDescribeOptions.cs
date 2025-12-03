using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "backups", "describe")]
public record GcloudActiveDirectoryDomainsBackupsDescribeOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Domain
) : GcloudOptions;