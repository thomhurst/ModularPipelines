using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "get-iam-policy")]
public record GcloudActiveDirectoryDomainsGetIamPolicyOptions(
[property: CliArgument] string Domain
) : GcloudOptions;