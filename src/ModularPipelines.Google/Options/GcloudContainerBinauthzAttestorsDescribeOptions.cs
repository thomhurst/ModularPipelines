using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestors", "describe")]
public record GcloudContainerBinauthzAttestorsDescribeOptions(
[property: PositionalArgument] string Attestor
) : GcloudOptions;