using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-templates", "get-iam-policy")]
public record GcloudComputeSoleTenancyNodeTemplatesGetIamPolicyOptions(
[property: CliArgument] string NodeTemplate,
[property: CliArgument] string Region
) : GcloudOptions;