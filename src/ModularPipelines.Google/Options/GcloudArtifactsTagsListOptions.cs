using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "tags", "list")]
public record GcloudArtifactsTagsListOptions(
[property: CliOption("--package")] string Package,
[property: CliOption("--location")] string Location,
[property: CliOption("--repository")] string Repository
) : GcloudOptions;