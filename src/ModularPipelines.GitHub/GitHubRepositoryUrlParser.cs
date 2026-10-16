namespace ModularPipelines.GitHub;

internal static class GitHubRepositoryUrlParser
{
    public static bool TryParse(
        string? remoteUrl,
        out string owner,
        out string repositoryName,
        string? serverUrl = null)
    {
        owner = string.Empty;
        repositoryName = string.Empty;

        if (string.IsNullOrWhiteSpace(remoteUrl)
            || !Uri.TryCreate(serverUrl ?? "https://github.com", UriKind.Absolute, out var server)
            || (server.Scheme != Uri.UriSchemeHttps && server.Scheme != Uri.UriSchemeHttp))
        {
            return false;
        }

        var normalizedUrl = remoteUrl.Trim();
        var sshPrefix = $"git@{server.Host}:";
        string repositoryPath;

        if (normalizedUrl.StartsWith(sshPrefix, StringComparison.OrdinalIgnoreCase))
        {
            repositoryPath = normalizedUrl[sshPrefix.Length..];
        }
        else if (Uri.TryCreate(normalizedUrl, UriKind.Absolute, out var uri)
                 && uri.Host.Equals(server.Host, StringComparison.OrdinalIgnoreCase)
                 && ((uri.Scheme == server.Scheme && uri.Port == server.Port)
                     || uri.Scheme == "ssh"))
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
