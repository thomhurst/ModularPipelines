using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute")]
public class GcloudComputeSoleTenancy
{
    public GcloudComputeSoleTenancy(
        GcloudComputeSoleTenancyNodeGroups nodeGroups,
        GcloudComputeSoleTenancyNodeTemplates nodeTemplates,
        GcloudComputeSoleTenancyNodeTypes nodeTypes
    )
    {
        NodeGroups = nodeGroups;
        NodeTemplates = nodeTemplates;
        NodeTypes = nodeTypes;
    }

    public GcloudComputeSoleTenancyNodeGroups NodeGroups { get; }

    public GcloudComputeSoleTenancyNodeTemplates NodeTemplates { get; }

    public GcloudComputeSoleTenancyNodeTypes NodeTypes { get; }
}