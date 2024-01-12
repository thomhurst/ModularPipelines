using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "packages", "describe")]
public record GcloudArtifactsPackagesDescribeOptions(
[property: PositionalArgument] string Package,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Repository
) : GcloudOptions;