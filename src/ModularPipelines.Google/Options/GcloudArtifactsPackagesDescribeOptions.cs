using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "packages", "describe")]
public record GcloudArtifactsPackagesDescribeOptions(
[property: CliArgument] string Package,
[property: CliArgument] string Location,
[property: CliArgument] string Repository
) : GcloudOptions;