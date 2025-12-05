using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-types", "describe")]
public record GcloudComputeSoleTenancyNodeTypesDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}