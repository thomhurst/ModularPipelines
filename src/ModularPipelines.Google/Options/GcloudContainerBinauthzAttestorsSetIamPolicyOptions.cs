using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestors", "set-iam-policy")]
public record GcloudContainerBinauthzAttestorsSetIamPolicyOptions(
[property: PositionalArgument] string AttestorName,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;