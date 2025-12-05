using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "set-iam-policy")]
public record GcloudActiveDirectoryDomainsSetIamPolicyOptions(
[property: CliArgument] string Domain,
[property: CliArgument] string PolicyFile
) : GcloudOptions;