namespace ModularPipelines.GitHub;

public interface IGitHubEnvironmentVariables
{
    bool CI { get; }
    
    string? Workflow { get; }
    
    string? RunId { get; }
    
    int RunNumber { get; }
    
    string? Action { get; }
    
    string? Actions { get; }
    
    string? Actor { get; }
    
    string? Repository { get; }
    
    string? EventName { get; }
    
    string? EventPath { get; }
    
    string? Workspace { get; }
    
    string? Sha { get; }
    
    string? Ref { get; }
    
    string? HeadRef { get; }
    
    string? BaseRef { get; }
    
    string? ServerUrl { get; }
    
    string? ApiUrl { get; }
    
    string? GraphQlUrl { get; }
    
    string? RefName { get; }
    
    string? RefType { get; }
    
    string? Token { get; }
    
    string? Job { get; }
    
    string? State { get; }
    
    string? ActorId { get; }
    
    string? Owner { get; }
    
    string? OwnerId { get; }
    
    string? RetentionDays { get; }
    
    string? RefProtected { get; }
    
    string? RepositoryId { get; }
    
    string? RunAttempt { get; }
    
    string? TriggeringActor { get; }
    
    string? WorkflowSha { get; }
    
    string? WorkflowRef { get; }
}