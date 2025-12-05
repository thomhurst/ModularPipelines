using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "repositories", "describe")]
public record GcloudBuildsRepositoriesDescribeOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Connection,
[property: CliArgument] string Region
) : GcloudOptions;