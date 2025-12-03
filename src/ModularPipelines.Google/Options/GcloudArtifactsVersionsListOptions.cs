using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "versions", "list")]
public record GcloudArtifactsVersionsListOptions(
[property: CliOption("--package")] string Package,
[property: CliOption("--location")] string Location,
[property: CliOption("--repository")] string Repository
) : GcloudOptions;