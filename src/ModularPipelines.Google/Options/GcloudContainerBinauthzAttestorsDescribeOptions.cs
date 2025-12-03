using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestors", "describe")]
public record GcloudContainerBinauthzAttestorsDescribeOptions(
[property: CliArgument] string Attestor
) : GcloudOptions;