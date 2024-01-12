using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("source", "repos", "set-iam-policy")]
public record GcloudSourceReposSetIamPolicyOptions(
[property: PositionalArgument] string RepositoryName,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;