using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("source", "repos", "set-iam-policy")]
public record GcloudSourceReposSetIamPolicyOptions(
[property: CliArgument] string RepositoryName,
[property: CliArgument] string PolicyFile
) : GcloudOptions;