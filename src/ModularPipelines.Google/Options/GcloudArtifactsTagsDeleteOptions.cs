using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "tags", "delete")]
public record GcloudArtifactsTagsDeleteOptions(
[property: PositionalArgument] string Tag,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Package,
[property: PositionalArgument] string Repository
) : GcloudOptions;