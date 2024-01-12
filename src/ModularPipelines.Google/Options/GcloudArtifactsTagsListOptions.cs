using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "tags", "list")]
public record GcloudArtifactsTagsListOptions(
[property: CommandSwitch("--package")] string Package,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--repository")] string Repository
) : GcloudOptions;