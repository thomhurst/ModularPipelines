using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

public interface IPipelineRequirement
{
    Task<RequirementDecision> MustAsync(IPipelineHookContext context);
}
