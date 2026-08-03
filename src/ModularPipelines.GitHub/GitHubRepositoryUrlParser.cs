namespace ModularPipelines.GitHub;

internal static class GitHubRepositoryUrlParser
{
    private const string GitHubHost = "github.com";
    private const string SshPrefix = "git@github.com:";

    public static bool TryParse(
        string remoteUrl,
        out string owner,
        out string repositoryName)
    {
        owner = string.Empty;
        repositoryName = string.Empty;

        var normalizedUrl = remoteUrl.Trim();
        string repositoryPath;

        if (normalizedUrl.StartsWith(SshPrefix, StringComparison.OrdinalIgnoreCase))
        {
            repositoryPath = normalizedUrl[SshPrefix.Length..];
        }
        else if (Uri.TryCreate(normalizedUrl, UriKind.Absolute, out var uri)
                 && uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
                 && uri.Host.Equals(GitHubHost, StringComparison.OrdinalIgnoreCase))
        {
            repositoryPath = uri.AbsolutePath;
        }
        else
        {
            return false;
        }

        var pathSegments = repositoryPath
            .Trim('/')
            .Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (pathSegments.Length != 2)
        {
            return false;
        }

        owner = Uri.UnescapeDataString(pathSegments[0]);
        repositoryName = Uri.UnescapeDataString(pathSegments[1]);
        if (repositoryName.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
        {
            repositoryName = repositoryName[..^4];
        }

        if (!string.IsNullOrWhiteSpace(owner)
            && !string.IsNullOrWhiteSpace(repositoryName))
        {
            return true;
        }

        owner = string.Empty;
        repositoryName = string.Empty;
        return false;
    }
}
