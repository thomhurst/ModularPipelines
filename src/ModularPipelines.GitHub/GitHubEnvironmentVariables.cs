namespace ModularPipelines.GitHub;

internal class GitHubEnvironmentVariables : IGitHubEnvironmentVariables
{
    public bool CI { get; } = bool.TryParse(Environment.GetEnvironmentVariable("CI"), out var ci) && ci;
    public string Workflow { get; } = Get("WORKFLOW");
    public string RunId { get; } = Get("RUN_ID");
    public int RunNumber { get; } = int.Parse(Get("RUN_NUMBER"));
    public string Action { get; } = Get("ACTION");
    public string Actions { get; } = Get("ACTIONS");
    public string Actor { get; } = Get("ACTOR");
    public string Repository { get; } = Get("REPOSITORY");
    public string EventName { get; } = Get("EVENT_NAME");
    public string EventPath { get; } = Get("EVENTPATH");
    public string Workspace { get; } = Get("WORKSPACE");
    public string Sha { get; } = Get("SHA");
    public string Ref { get; } = Get("REF");
    public string HeadRef { get; } = Get("HEAD_REF");
    public string BaseRef { get; } = Get("BASE_REF");
    public string ServerUrl { get; } = Get("SERVER_URL");
    public string ApiUrl { get; } = Get("API_URL");
    public string GraphQlUrl { get; } = Get("GRAPHQL_URL");

    private static string Get(string name) => Environment.GetEnvironmentVariable($"GITHUB_{name}")!;
}