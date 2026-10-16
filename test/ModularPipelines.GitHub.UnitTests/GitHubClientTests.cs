using System.Net;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.GitHub;
using Moq;

namespace ModularPipelines.GitHub.UnitTests;

public class GitHubClientTests
{
    [Test]
    [Arguments(null, "https://api.github.com/")]
    [Arguments("https://github.example.com/api/v3/", "https://github.example.com/api/v3/")]
    [Arguments("https://github.example.com/api/v3", "https://github.example.com/api/v3/")]
    public async Task Client_Uses_Configured_Api_Url(string? apiUrl, string expected)
    {
        var environment = new Mock<IGitHubEnvironmentVariables>();
        environment.SetupGet(x => x.ApiUrl).Returns(apiUrl);
        using var handler = new RecordingHandler();
        var factory = new Mock<IHttpMessageHandlerFactory>();
        factory.Setup(x => x.CreateHandler(GitHubHttpClientServiceCollectionExtensions.GitHubClientName)).Returns(handler);
        var github = new GitHub(
            Microsoft.Extensions.Options.Options.Create(new GitHubOptions { AccessToken = "test-token" }),
            environment.Object,
            Mock.Of<IGitHubRepositoryInfo>(),
            factory.Object,
            Mock.Of<IConsoleCoordinator>(),
            Mock.Of<IBuildSystemFormatterProvider>());

        await github.Client.Repository.Get("owner", "repository");

        await Assert.That(handler.RequestUri).IsEqualTo(new Uri($"{expected}repos/owner/repository"));
        await Assert.That(handler.Authorization).IsEqualTo("Token test-token");
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public Uri? RequestUri { get; private set; }

        public string? Authorization { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri;
            Authorization = request.Headers.Authorization?.ToString();
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json"),
            });
        }
    }
}
