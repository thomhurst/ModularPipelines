using ModularPipelines.Exceptions;
using ModularPipelines.Requirements;

namespace ModularPipelines.Engine;

public class RequirementChecker : IRequirementChecker
{
    private readonly List<IPipelineRequirement> _requirements;

    public RequirementChecker( IEnumerable<IPipelineRequirement> requirements )
    {
        _requirements = requirements.ToList();
    }
    public async Task CheckRequirementsAsync()
    {
        var failedRequirementsNames = new List<string>();

        var requirementTasks = _requirements.Select( x => x.MustAsync() ).ToList();

        for (var index = 0; index < requirementTasks.Count; index++)
        {
            var requirement = _requirements[index];
            var requirementTask = requirementTasks[index];
            if (!await requirementTask)
            {
                failedRequirementsNames.Add( requirement.GetType().Name );
            }
        }

        if (failedRequirementsNames.Any())
        {
            throw new FailedRequirementsException( $"Requirements failed: {string.Join( " | ", failedRequirementsNames )}" );
        }
    }
}
