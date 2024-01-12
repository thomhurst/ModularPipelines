using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("projects", "update")]
public record GcloudProjectsUpdateOptions(
[property: PositionalArgument] string ProjectId,
[property: CommandSwitch("--name")] string Name
) : GcloudOptions;