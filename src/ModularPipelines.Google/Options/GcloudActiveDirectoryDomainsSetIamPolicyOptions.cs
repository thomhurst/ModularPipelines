using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "set-iam-policy")]
public record GcloudActiveDirectoryDomainsSetIamPolicyOptions(
[property: PositionalArgument] string Domain,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;