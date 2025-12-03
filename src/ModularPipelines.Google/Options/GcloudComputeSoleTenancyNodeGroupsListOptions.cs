using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-groups", "list")]
public record GcloudComputeSoleTenancyNodeGroupsListOptions : GcloudOptions
{
    [CliFlag("--share-settings")]
    public bool? ShareSettings { get; set; }
}