using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-templates", "set-iam-policy")]
public record GcloudComputeInstanceTemplatesSetIamPolicyOptions(
[property: CliArgument] string InstanceTemplate,
[property: CliArgument] string PolicyFile
) : GcloudOptions;