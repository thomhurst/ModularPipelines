using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-templates", "add-iam-policy-binding")]
public record GcloudComputeSoleTenancyNodeTemplatesAddIamPolicyBindingOptions(
[property: CliArgument] string NodeTemplate,
[property: CliArgument] string Region,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;