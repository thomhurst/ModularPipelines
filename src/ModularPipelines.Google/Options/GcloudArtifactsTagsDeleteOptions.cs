using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "tags", "delete")]
public record GcloudArtifactsTagsDeleteOptions(
[property: CliArgument] string Tag,
[property: CliArgument] string Location,
[property: CliArgument] string Package,
[property: CliArgument] string Repository
) : GcloudOptions;