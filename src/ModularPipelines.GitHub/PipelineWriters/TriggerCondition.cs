using YamlDotNet.Serialization;

namespace ModularPipelines.GitHub.PipelineWriters;

public record TriggerCondition
{
    public TriggerConditionBranches? Push { get; init; }
    
    [YamlMember(Alias = "pull_request", ApplyNamingConventions = false)]
    public TriggerConditionBranches? PullRequest { get; init; }
    
    [YamlMember(Alias = "workflow_dispatch", ApplyNamingConventions = false)]
    public WorkflowDispatch? WorkflowDispatch { get; init; }
}