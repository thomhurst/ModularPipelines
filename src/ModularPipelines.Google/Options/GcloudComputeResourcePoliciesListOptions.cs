using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "resource-policies", "list")]
public record GcloudComputeResourcePoliciesListOptions : GcloudOptions;