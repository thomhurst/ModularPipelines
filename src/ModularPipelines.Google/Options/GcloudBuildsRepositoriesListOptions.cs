using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "repositories", "list")]
public record GcloudBuildsRepositoriesListOptions(
[property: CliOption("--connection")] string Connection,
[property: CliOption("--region")] string Region
) : GcloudOptions;