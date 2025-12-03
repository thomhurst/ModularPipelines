using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "describe")]
public record GcloudActiveDirectoryDomainsDescribeOptions(
[property: CliArgument] string Domain
) : GcloudOptions;