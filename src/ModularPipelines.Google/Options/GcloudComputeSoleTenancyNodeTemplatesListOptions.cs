using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-templates", "list")]
public record GcloudComputeSoleTenancyNodeTemplatesListOptions : GcloudOptions;