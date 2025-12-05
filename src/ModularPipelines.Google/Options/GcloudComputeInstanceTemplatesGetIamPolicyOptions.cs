using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-templates", "get-iam-policy")]
public record GcloudComputeInstanceTemplatesGetIamPolicyOptions(
[property: CliArgument] string InstanceTemplate
) : GcloudOptions;