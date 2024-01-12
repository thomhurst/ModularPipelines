using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "reset-admin-password")]
public record GcloudActiveDirectoryDomainsResetAdminPasswordOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions;