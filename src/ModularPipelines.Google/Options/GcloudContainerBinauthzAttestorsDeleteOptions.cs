using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestors", "delete")]
public record GcloudContainerBinauthzAttestorsDeleteOptions(
[property: CliArgument] string Attestor
) : GcloudOptions;