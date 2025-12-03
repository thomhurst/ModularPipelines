using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "repositories", "describe")]
public record GcloudArtifactsRepositoriesDescribeOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location
) : GcloudOptions;