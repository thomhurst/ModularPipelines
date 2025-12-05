using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "reset-admin-password")]
public record GcloudActiveDirectoryDomainsResetAdminPasswordOptions(
[property: CliArgument] string Domain
) : GcloudOptions;