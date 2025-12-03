using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "machine-images", "add-iam-policy-binding")]
public record GcloudComputeMachineImagesAddIamPolicyBindingOptions(
[property: CliArgument] string MachineImage,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;