using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "describe-ldaps-settings")]
public record GcloudActiveDirectoryDomainsDescribeLdapsSettingsOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions;