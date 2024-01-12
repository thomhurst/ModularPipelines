using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "policy", "get-iam-policy")]
public record GcloudContainerBinauthzPolicyGetIamPolicyOptions : GcloudOptions;