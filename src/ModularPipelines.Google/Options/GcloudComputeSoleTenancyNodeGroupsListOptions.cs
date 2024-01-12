using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-groups", "list")]
public record GcloudComputeSoleTenancyNodeGroupsListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--share-settings")]
    public bool? ShareSettings { get; set; }
}