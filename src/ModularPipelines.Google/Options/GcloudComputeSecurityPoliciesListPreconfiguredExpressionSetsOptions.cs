using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "list-preconfigured-expression-sets")]
public record GcloudComputeSecurityPoliciesListPreconfiguredExpressionSetsOptions : GcloudOptions;