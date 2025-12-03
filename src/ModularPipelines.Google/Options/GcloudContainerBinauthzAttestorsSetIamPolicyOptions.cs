using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestors", "set-iam-policy")]
public record GcloudContainerBinauthzAttestorsSetIamPolicyOptions(
[property: CliArgument] string AttestorName,
[property: CliArgument] string PolicyFile
) : GcloudOptions;