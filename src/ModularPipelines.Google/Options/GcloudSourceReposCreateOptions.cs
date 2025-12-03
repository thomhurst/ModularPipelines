using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("source", "repos", "create")]
public record GcloudSourceReposCreateOptions(
[property: CliArgument] string RepositoryName
) : GcloudOptions;