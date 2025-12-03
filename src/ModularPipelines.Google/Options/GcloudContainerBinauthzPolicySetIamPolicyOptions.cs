using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "policy", "set-iam-policy")]
public record GcloudContainerBinauthzPolicySetIamPolicyOptions(
[property: CliArgument] string PolicyFile
) : GcloudOptions;