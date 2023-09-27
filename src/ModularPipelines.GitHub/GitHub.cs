namespace ModularPipelines.GitHub;

internal class GitHub : IGitHub
{
    public GitHub(IGitHubEnvironmentVariables environmentVariables)
    {
        EnvironmentVariables = environmentVariables;
    }

    public IGitHubEnvironmentVariables EnvironmentVariables { get; }
}