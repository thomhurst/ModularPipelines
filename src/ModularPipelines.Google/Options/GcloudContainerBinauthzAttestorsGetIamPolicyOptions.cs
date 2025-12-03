using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestors", "get-iam-policy")]
public record GcloudContainerBinauthzAttestorsGetIamPolicyOptions(
[property: CliArgument] string Attestor
) : GcloudOptions;