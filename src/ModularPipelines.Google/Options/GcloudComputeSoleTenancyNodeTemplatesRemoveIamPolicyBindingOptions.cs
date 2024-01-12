using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-templates", "remove-iam-policy-binding")]
public record GcloudComputeSoleTenancyNodeTemplatesRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string NodeTemplate,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;