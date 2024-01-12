using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("components", "remove")]
public record GcloudComponentsRemoveOptions(
[property: PositionalArgument] string ComponentId
) : GcloudOptions;