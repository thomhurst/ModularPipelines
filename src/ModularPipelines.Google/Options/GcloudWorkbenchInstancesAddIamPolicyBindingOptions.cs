using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workbench", "instances", "add-iam-policy-binding")]
public record GcloudWorkbenchInstancesAddIamPolicyBindingOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;