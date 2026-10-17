namespace ModularPipelines.GitHub;

internal static class GitHubAccessToken
{
    public static string? Resolve(string? configuredToken, string? environmentToken)
    {
        if (!string.IsNullOrWhiteSpace(configuredToken))
        {
            return configuredToken;
        }

        return string.IsNullOrWhiteSpace(environmentToken) ? null : environmentToken;
    }
}
