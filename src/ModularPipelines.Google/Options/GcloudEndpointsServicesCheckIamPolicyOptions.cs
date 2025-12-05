using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "services", "check-iam-policy")]
public record GcloudEndpointsServicesCheckIamPolicyOptions(
[property: CliArgument] string Service
) : GcloudOptions;