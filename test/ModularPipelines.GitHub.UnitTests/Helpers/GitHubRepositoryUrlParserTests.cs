using ModularPipelines.GitHub;

namespace ModularPipelines.GitHub.UnitTests.Helpers;

public class GitHubRepositoryUrlParserTests
{
    [Test]
    [Arguments("https://github.com/owner/repository.git")]
    [Arguments("https://github.com/owner/repository")]
    [Arguments("https://github.com/owner/repository.git/")]
    [Arguments("https://username:token@github.com/owner/repository.git")]
    [Arguments("git@github.com:owner/repository.git")]
    [Arguments("git@github.com:owner/repository")]
    public async Task Supported_Remote_Urls_Are_Parsed(string remoteUrl)
    {
        var parsed = GitHubRepositoryUrlParser.TryParse(
            remoteUrl,
            out var owner,
            out var repositoryName);

        using (Assert.Multiple())
        {
            await Assert.That(parsed).IsTrue();
            await Assert.That(owner).IsEqualTo("owner");
            await Assert.That(repositoryName).IsEqualTo("repository");
        }
    }

    [Test]
    [Arguments("")]
    [Arguments("https://gitlab.com/owner/repository.git")]
    [Arguments("https://github.com/owner")]
    [Arguments("https://github.com/owner/repository/extra")]
    public async Task Unsupported_Remote_Urls_Are_Rejected(string remoteUrl)
    {
        var parsed = GitHubRepositoryUrlParser.TryParse(
            remoteUrl,
            out var owner,
            out var repositoryName);

        using (Assert.Multiple())
        {
            await Assert.That(parsed).IsFalse();
            await Assert.That(owner).IsEmpty();
            await Assert.That(repositoryName).IsEmpty();
        }
    }

    [Test]
    [Arguments("https://github.example.com/owner/repository.git")]
    [Arguments("git@github.example.com:owner/repository.git")]
    [Arguments("ssh://git@github.example.com/owner/repository.git")]
    public async Task Enterprise_Remote_Urls_Are_Parsed(string remoteUrl)
    {
        var parsed = GitHubRepositoryUrlParser.TryParse(
            remoteUrl, out var owner, out var repositoryName, "https://github.example.com");

        using (Assert.Multiple())
        {
            await Assert.That(parsed).IsTrue();
            await Assert.That(owner).IsEqualTo("owner");
            await Assert.That(repositoryName).IsEqualTo("repository");
        }
    }

    [Test]
    [Arguments("https://github.com/owner/repository.git")]
    [Arguments("git@github.com:owner/repository.git")]
    [Arguments("https://github.example.com.evil.test/owner/repository.git")]
    public async Task Other_Hosts_Are_Rejected_For_Enterprise(string remoteUrl)
    {
        await Assert.That(GitHubRepositoryUrlParser.TryParse(
            remoteUrl, out _, out _, "https://github.example.com")).IsFalse();
    }
}
