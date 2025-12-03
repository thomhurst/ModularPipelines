using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-templates", "set-iam-policy")]
public record GcloudComputeSoleTenancyNodeTemplatesSetIamPolicyOptions(
[property: CliArgument] string NodeTemplate,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;