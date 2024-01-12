using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "delete")]
public record GcloudComputeNetworksDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;