using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("source", "repos", "create")]
public record GcloudSourceReposCreateOptions(
[property: PositionalArgument] string RepositoryName
) : GcloudOptions;