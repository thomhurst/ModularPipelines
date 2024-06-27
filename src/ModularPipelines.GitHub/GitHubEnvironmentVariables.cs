using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.GitHub;

[ExcludeFromCodeCoverage]
internal class GitHubEnvironmentVariables : IGitHubEnvironmentVariables
{
    public bool CI { get; } = bool.TryParse(Environment.GetEnvironmentVariable("CI"), out var ci) && ci;

    public string? Workflow { get; } = Get("WORKFLOW");

    public string? RunId { get; } = Get("RUN_ID");

    public int RunNumber => int.Parse(Get("RUN_NUMBER"));

    public string? Action { get; } = Get("ACTION");

    public string? Actions { get; } = Get("ACTIONS");

    public string? Actor { get; } = Get("ACTOR");

    public string? Repository { get; } = Get("REPOSITORY");

    public string? EventName { get; } = Get("EVENT_NAME");

    public string? EventPath { get; } = Get("EVENTPATH");

    public string? Workspace { get; } = Get("WORKSPACE");

    public string? Sha { get; } = Get("SHA");

    public string? Ref { get; } = Get("REF");

    public string? HeadRef { get; } = Get("HEAD_REF");

    public string? BaseRef { get; } = Get("BASE_REF");

    public string? ServerUrl { get; } = Get("SERVER_URL");

    public string? ApiUrl { get; } = Get("API_URL");

    public string? GraphQlUrl { get; } = Get("GRAPHQL_URL");
    
    public string? RefName { get; } = Get("REF_NAME");

    public string? RefType { get; } = Get("REF_TYPE");

    public string? Token { get; } = Get("TOKEN");

    public string? Job { get; } = Get("JOB");
    
    public string? State { get; } = Get("STATE");

    public string? ActorId { get; } = Get("ACTOR_ID");

    public string? Owner { get; } = Get("REPOSITORY_OWNER");

    public string? OwnerId { get; } = Get("REPOSITORY_OWNER_ID");

    public string? RetentionDays { get; } = Get("RETENTION_DAYS");

    public string? RefProtected { get; } = Get("REF_PROTECTED");

    public string? RepositoryId { get; } = Get("REPOSITORY_ID");

    public string? RunAttempt { get; } = Get("RUN_ATTEMPT");

    public string? TriggeringActor { get; } = Get("TRIGGERING_ACTOR");
    
    public string? WorkflowSha { get; } = Get("WORKFLOW_SHA");

    public string? WorkflowRef { get; } = Get("WORKFLOW_REF");
    
    private static string Get(string name) => Environment.GetEnvironmentVariable($"GITHUB_{name}")!;
}