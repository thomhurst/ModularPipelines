using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routes", "delete")]
public record GcloudComputeRoutesDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;