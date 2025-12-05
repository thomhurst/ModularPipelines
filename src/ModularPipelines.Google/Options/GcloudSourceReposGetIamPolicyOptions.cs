using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("source", "repos", "get-iam-policy")]
public record GcloudSourceReposGetIamPolicyOptions(
[property: CliArgument] string RepositoryName
) : GcloudOptions;