using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "delete")]
public record GcloudComputeInterconnectsDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;