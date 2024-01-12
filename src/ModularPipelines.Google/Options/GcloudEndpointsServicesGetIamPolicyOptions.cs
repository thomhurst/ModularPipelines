using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("endpoints", "services", "get-iam-policy")]
public record GcloudEndpointsServicesGetIamPolicyOptions(
[property: PositionalArgument] string Service
) : GcloudOptions;