using System.Net;
using ModularPipelines.Constants;
using ModularPipelines.Exceptions;
using ModularPipelines.Options;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class HttpUriRedactionTests
{
    [Test]
    [Arguments("sig")]
    [Arguments("SIG")]
    [Arguments("X-Amz-Signature")]
    [Arguments("X-Amz-Credential")]
    [Arguments("X-Amz-Security-Token")]
    [Arguments("X-Goog-Signature")]
    [Arguments("X-Goog-Credential")]
    [Arguments("token")]
    [Arguments("access_token")]
    [Arguments("code")]
    [Arguments("%74oken")]
    public async Task Display_Redacts_Sensitive_Query_Values(string name)
    {
        var uri = new Uri($"https://example.test/file?keep=a%2Fb&{name}=first-secret&{name}=second%2Bsecret&empty=#section");
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        var formatted = await HttpFormatterTests.CreateRequestFormatter().FormatAsync(request);
        var exception = new PipelineHttpResponseException(HttpStatusCode.BadRequest, "Bad Request", null, uri);

        foreach (var display in new[] { formatted, exception.Message, exception.ToString() })
        {
            await Assert.That(display).DoesNotContain("first-secret");
            await Assert.That(display).DoesNotContain("second");
            await Assert.That(display).Contains($"{Uri.UnescapeDataString(name)}={LoggingConstants.SecretMask}");
            await Assert.That(display).Contains("keep=a%2Fb");
            await Assert.That(display).Contains("empty=#section");
        }

        await Assert.That(exception.RequestUri).IsSameReferenceAs(uri);
        await Assert.That(uri.Query).Contains("first-secret");
    }

    [Test]
    [Arguments("https://user:password@example.test:8443/file?keep=yes")]
    [Arguments("https://user%40name:pass%3Aword@example.test/file")]
    public async Task Display_Redacts_UserInfo(string url)
    {
        var uri = new Uri(url);
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        var formatted = await HttpFormatterTests.CreateRequestFormatter().FormatAsync(request);
        var exception = new PipelineHttpResponseException(HttpStatusCode.BadRequest, null, null, uri, new InvalidOperationException("failure"));

        foreach (var display in new[] { formatted, exception.Message })
        {
            await Assert.That(display).DoesNotContain("user");
            await Assert.That(display).DoesNotContain("pass");
            await Assert.That(display).Contains($"{LoggingConstants.SecretMask}@example.test");
        }

        await Assert.That(request.RequestUri).IsSameReferenceAs(uri);
    }

    [Test]
    [Arguments("https://hooks.slack.com/services/team/channel/webhook-secret")]
    [Arguments("https://hooks.slack-gov.com/services/team/channel/webhook-secret")]
    [Arguments("https://tenant.webhook.office.com/webhookb2/webhook-secret/IncomingWebhook/key")]
    [Arguments("https://outlook.office.com/webhook/webhook-secret/IncomingWebhook/key")]
    [Arguments("https://outlook.office365.com/webhook/webhook-secret/IncomingWebhook/key")]
    public async Task Display_Redacts_Webhook_Paths(string url)
    {
        var uri = new Uri(url);
        using var request = new HttpRequestMessage(HttpMethod.Post, uri);
        var formatted = await HttpFormatterTests.CreateRequestFormatter().FormatAsync(request);
        var exception = new PipelineHttpResponseException(HttpStatusCode.BadRequest, null, null, uri);

        foreach (var display in new[] { formatted, exception.Message })
        {
            await Assert.That(display).DoesNotContain("webhook-secret");
            await Assert.That(display).Contains($"https://{uri.Host}/{LoggingConstants.SecretMask}");
        }
    }

    [Test]
    [Arguments("https://example.test/file?api_key=custom-secret")]
    [Arguments("/file?API_KEY=custom-secret")]
    public async Task Formatter_Uses_Custom_Query_Names_Without_Changing_Request(string url)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url, UriKind.RelativeOrAbsolute));
        var original = request.RequestUri!.OriginalString;
        var names = new List<string>(HttpLoggingOptions.Default.SensitiveQueryParameterNames) { "api_key" };
        var options = new HttpLoggingOptions { SensitiveQueryParameterNames = names };
        names.Clear();

        var formatted = await HttpFormatterTests.CreateRequestFormatter().FormatAsync(request, options);

        await Assert.That(formatted).DoesNotContain("custom-secret");
        await Assert.That(formatted).Contains(LoggingConstants.SecretMask);
        await Assert.That(request.RequestUri.OriginalString).IsEqualTo(original);
        await Assert.That(options.SensitiveQueryParameterNames.Contains("token")).IsTrue();
    }

    [Test]
    [Arguments("https://example.test/path?keep=a%2Fb&flag&empty=#section")]
    [Arguments("/path?keep=a%2Fb&flag&empty=#section")]
    [Arguments("https://example.test/path#fragment?token=not-a-query")]
    [Arguments("https://hooks.slack.com.example.test/services/public")]
    [Arguments("https://outlook.office.com/mail/public")]
    public async Task Display_Preserves_NonSensitive_Uri(string url)
    {
        var uri = new Uri(url, UriKind.RelativeOrAbsolute);
        await Assert.That(HttpUriRedactor.Redact(uri)).IsEqualTo(url);
    }

    [Test]
    [Arguments("//user:password@example.test/file?token=secret", "//" + LoggingConstants.SecretMask + "@example.test/file?token=" + LoggingConstants.SecretMask)]
    [Arguments("//hooks.slack.com/services/team/channel/secret", "//hooks.slack.com/" + LoggingConstants.SecretMask)]
    public async Task Display_Redacts_Network_Path_Reference(string url, string expected)
    {
        var uri = new Uri(url, UriKind.Relative);
        await Assert.That(HttpUriRedactor.Redact(uri)).IsEqualTo(expected);
        await Assert.That(uri.OriginalString).IsEqualTo(url);
    }

    [Test]
    public async Task Display_Handles_Missing_Uri()
    {
        await Assert.That(HttpUriRedactor.Redact(null)).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Http_Failure_Uses_Effective_Query_Names(bool perRequest)
    {
        var uri = new Uri("https://example.test/failure?api_key=custom-secret&token=default-secret");
        var logging = HttpLoggingOptions.Silent with
        {
            SensitiveQueryParameterNames = [.. HttpLoggingOptions.Default.SensitiveQueryParameterNames, "api_key"],
        };
        var pipelineOptions = new PipelineOptions
        {
            Http = new PipelineHttpOptions { Logging = perRequest ? null : logging },
        };
        using var client = new HttpClient(new FailureHandler());
        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);
        var http = new ModularPipelines.Http.Http(factory.Object, Mock.Of<IModuleLoggerAccessor>(),
            Mock.Of<IHttpLogger>(), Microsoft.Extensions.Options.Options.Create(pipelineOptions));
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);

        var exception = await Assert.ThrowsAsync<PipelineHttpResponseException>(() => http.SendAsync(new HttpOptions(request)
        {
            Logging = perRequest ? logging : null,
        }));

        await Assert.That(exception!.Message).DoesNotContain("custom-secret");
        await Assert.That(exception.Message).DoesNotContain("default-secret");
        await Assert.That(exception.Message).Contains($"api_key={LoggingConstants.SecretMask}");
        await Assert.That(exception.RequestUri).IsSameReferenceAs(uri);
        await Assert.That(request.RequestUri).IsSameReferenceAs(uri);
    }

    private sealed class FailureHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = request,
                Content = new StringContent("failure"),
            });
        }
    }
}
