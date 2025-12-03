using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "describe-ldaps-settings")]
public record GcloudActiveDirectoryDomainsDescribeLdapsSettingsOptions(
[property: CliArgument] string Domain
) : GcloudOptions;