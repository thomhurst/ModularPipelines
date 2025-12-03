using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "services", "get-iam-policy")]
public record GcloudEndpointsServicesGetIamPolicyOptions(
[property: CliArgument] string Service
) : GcloudOptions;