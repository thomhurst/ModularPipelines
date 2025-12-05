using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "resource-policies", "list")]
public record GcloudComputeResourcePoliciesListOptions : GcloudOptions;