using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-templates", "delete")]
public record GcloudComputeSoleTenancyNodeTemplatesDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}