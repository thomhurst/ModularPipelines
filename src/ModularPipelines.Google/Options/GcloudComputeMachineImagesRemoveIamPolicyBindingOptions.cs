using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "machine-images", "remove-iam-policy-binding")]
public record GcloudComputeMachineImagesRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string MachineImage,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;