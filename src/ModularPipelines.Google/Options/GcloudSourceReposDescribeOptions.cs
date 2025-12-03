using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("source", "repos", "describe")]
public record GcloudSourceReposDescribeOptions(
[property: CliArgument] string RepositoryName
) : GcloudOptions;