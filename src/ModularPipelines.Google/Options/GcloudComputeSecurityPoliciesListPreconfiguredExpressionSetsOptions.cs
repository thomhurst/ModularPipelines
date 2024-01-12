using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "list-preconfigured-expression-sets")]
public record GcloudComputeSecurityPoliciesListPreconfiguredExpressionSetsOptions : GcloudOptions;