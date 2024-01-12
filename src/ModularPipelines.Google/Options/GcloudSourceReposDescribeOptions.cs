using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("source", "repos", "describe")]
public record GcloudSourceReposDescribeOptions(
[property: PositionalArgument] string RepositoryName
) : GcloudOptions;