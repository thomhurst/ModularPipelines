using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "machine-images", "set-iam-policy")]
public record GcloudComputeMachineImagesSetIamPolicyOptions(
[property: CliArgument] string MachineImage,
[property: CliArgument] string PolicyFile
) : GcloudOptions;