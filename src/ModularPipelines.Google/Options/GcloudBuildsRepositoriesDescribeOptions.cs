using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "repositories", "describe")]
public record GcloudBuildsRepositoriesDescribeOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region
) : GcloudOptions;