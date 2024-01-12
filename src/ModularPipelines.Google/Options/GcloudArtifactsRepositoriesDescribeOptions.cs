using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "repositories", "describe")]
public record GcloudArtifactsRepositoriesDescribeOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Location
) : GcloudOptions;