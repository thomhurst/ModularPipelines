using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestors", "delete")]
public record GcloudContainerBinauthzAttestorsDeleteOptions(
[property: PositionalArgument] string Attestor
) : GcloudOptions;