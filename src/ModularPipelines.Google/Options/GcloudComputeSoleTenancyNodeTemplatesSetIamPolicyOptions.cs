using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-templates", "set-iam-policy")]
public record GcloudComputeSoleTenancyNodeTemplatesSetIamPolicyOptions(
[property: PositionalArgument] string NodeTemplate,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;