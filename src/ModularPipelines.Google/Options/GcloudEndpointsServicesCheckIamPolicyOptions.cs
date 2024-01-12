using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("endpoints", "services", "check-iam-policy")]
public record GcloudEndpointsServicesCheckIamPolicyOptions(
[property: PositionalArgument] string Service
) : GcloudOptions;