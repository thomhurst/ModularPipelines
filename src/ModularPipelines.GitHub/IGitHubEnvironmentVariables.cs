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
}