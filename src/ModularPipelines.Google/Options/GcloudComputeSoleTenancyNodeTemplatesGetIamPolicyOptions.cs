using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-templates", "get-iam-policy")]
public record GcloudComputeSoleTenancyNodeTemplatesGetIamPolicyOptions(
[property: PositionalArgument] string NodeTemplate,
[property: PositionalArgument] string Region
) : GcloudOptions;