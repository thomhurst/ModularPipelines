using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestors", "get-iam-policy")]
public record GcloudContainerBinauthzAttestorsGetIamPolicyOptions(
[property: PositionalArgument] string Attestor
) : GcloudOptions;