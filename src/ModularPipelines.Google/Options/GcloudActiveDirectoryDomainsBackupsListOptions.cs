using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "backups", "list")]
public record GcloudActiveDirectoryDomainsBackupsListOptions(
[property: CliOption("--domain")] string Domain
) : GcloudOptions;