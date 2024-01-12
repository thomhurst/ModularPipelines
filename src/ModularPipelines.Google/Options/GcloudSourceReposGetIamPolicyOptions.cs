using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("source", "repos", "get-iam-policy")]
public record GcloudSourceReposGetIamPolicyOptions(
[property: PositionalArgument] string RepositoryName
) : GcloudOptions;