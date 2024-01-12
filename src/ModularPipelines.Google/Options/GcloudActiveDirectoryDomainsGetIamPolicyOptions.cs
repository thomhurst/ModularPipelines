using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "get-iam-policy")]
public record GcloudActiveDirectoryDomainsGetIamPolicyOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions;