using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "policy", "get-iam-policy")]
public record GcloudContainerBinauthzPolicyGetIamPolicyOptions : GcloudOptions;