using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "policy", "set-iam-policy")]
public record GcloudContainerBinauthzPolicySetIamPolicyOptions(
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;