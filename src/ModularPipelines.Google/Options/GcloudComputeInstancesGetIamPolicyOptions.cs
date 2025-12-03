using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "get-iam-policy")]
public record GcloudComputeInstancesGetIamPolicyOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Zone
) : GcloudOptions;