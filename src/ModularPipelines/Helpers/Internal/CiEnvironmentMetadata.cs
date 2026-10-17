namespace ModularPipelines.Helpers.Internal;

internal static class CiEnvironmentMetadata
{
    // Order preserves the OpenTelemetry SHA fallback precedence, independently of build-system detection.
    private static readonly Provider[] Providers =
    [
        new(BuildSystem.GitHubActions, ["GITHUB_HEAD_REF", "GITHUB_REF_NAME"], "GITHUB_SHA",
            read => read("GITHUB_REF_TYPE") == "tag"),
        new(BuildSystem.GitLab, ["CI_MERGE_REQUEST_SOURCE_BRANCH_NAME", "CI_COMMIT_REF_NAME"], "CI_COMMIT_SHA",
            read => read("CI_COMMIT_TAG") is not null),
        new(BuildSystem.AzurePipelines, ["SYSTEM_PULLREQUEST_SOURCEBRANCH", "BUILD_SOURCEBRANCH"], "BUILD_SOURCEVERSION"),
        new(BuildSystem.Bitbucket, ["BITBUCKET_BRANCH"], "BITBUCKET_COMMIT",
            read => read("BITBUCKET_TAG") is not null),
        new(null, [], "CODEBUILD_RESOLVED_SOURCE_VERSION"),
        new(BuildSystem.TravisCI, ["TRAVIS_PULL_REQUEST_BRANCH", "TRAVIS_BRANCH"], "TRAVIS_COMMIT",
            read => read("TRAVIS_TAG") is not null),
        new(BuildSystem.AppVeyor, ["APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH", "APPVEYOR_REPO_BRANCH"], "APPVEYOR_REPO_COMMIT",
            read => string.Equals(read("APPVEYOR_REPO_TAG"), "true", StringComparison.OrdinalIgnoreCase)
                    || read("APPVEYOR_REPO_TAG_NAME") is not null),
        new(BuildSystem.Jenkins, ["CHANGE_BRANCH", "BRANCH_NAME"], null,
            read => read("TAG_NAME") is not null),
    ];

    public static string? GetBranchName(BuildSystem buildSystem, Func<string, string?> readVariable)
    {
        string? Read(string name) => NullIfBlank(readVariable(name));

        var provider = Providers.FirstOrDefault(candidate => candidate.BuildSystem == buildSystem);
        if (provider is null || provider.IsTag?.Invoke(Read) == true)
        {
            return null;
        }

        return provider.BranchVariables.Select(Read).FirstOrDefault(value => value is not null);
    }

    public static string? FindCommitSha(Func<string, string?> readVariable)
    {
        return Providers
            .Where(provider => provider.CommitVariable is not null)
            .Select(provider => NullIfBlank(readVariable(provider.CommitVariable!)))
            .FirstOrDefault(value => value is not null);
    }

    private static string? NullIfBlank(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;

    private sealed record Provider(
        BuildSystem? BuildSystem,
        string[] BranchVariables,
        string? CommitVariable,
        Func<Func<string, string?>, bool>? IsTag = null);
}
