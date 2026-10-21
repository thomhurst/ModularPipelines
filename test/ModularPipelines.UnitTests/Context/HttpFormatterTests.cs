using ModularPipelines.Secrets;
using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;
using System.Net.Http.Headers;
using System.Text;
using ModularPipelines.Constants;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Http;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class HttpFormatterTests
{
    [Test]
    [MatrixDataSource]
    public async Task Formatters_RedactCredentialHeaders(
        [Matrix("X-NuGet-ApiKey", "X-Vault-Token", "PRIVATE-TOKEN", "Ocp-Apim-Subscription-Key", "X-JFrog-Art-Api", "DD-API-KEY")] string headerName,
        [Matrix(false, true)] bool copyDefaultHeaders)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test")
        {
            Content = new StringContent(string.Empty),
        };
        using var response = new HttpResponseMessage
        {
            Content = new StringContent(string.Empty),
        };
        var contentHeaderName = headerName.ToLowerInvariant();
        request.Headers.Add(headerName, ["request-credential-one", "request-credential-two"]);
        request.Content.Headers.Add(contentHeaderName, "request-content-credential");
        request.Headers.Add("X-Request-ID", "request-id");
        response.Headers.Add(headerName, ["response-credential-one", "response-credential-two"]);
        response.Content.Headers.Add(contentHeaderName, "response-content-credential");
        response.Headers.Add("X-Request-ID", "response-id");
        var options = copyDefaultHeaders
            ? new HttpLoggingOptions { SensitiveHeaderNames = [.. HttpLoggingOptions.DefaultSensitiveHeaders] }
            : HttpLoggingOptions.Default;

        var formattedRequest = await CreateRequestFormatter().FormatAsync(request, options);
        var formattedResponse = await CreateResponseFormatter().FormatAsync(response, options);

        using (Assert.Multiple())
        {
            await Assert.That(formattedRequest).Contains($"\t{headerName}: {LoggingConstants.SecretMask}");
            await Assert.That(formattedRequest).Contains($"\t{contentHeaderName}: {LoggingConstants.SecretMask}");
            await Assert.That(formattedRequest).DoesNotContain("request-credential");
            await Assert.That(formattedRequest).DoesNotContain("request-content-credential");
            await Assert.That(formattedRequest).Contains("X-Request-ID: request-id");
            await Assert.That(formattedResponse).Contains($"\t{headerName}: {LoggingConstants.SecretMask}");
            await Assert.That(formattedResponse).Contains($"\t{contentHeaderName}: {LoggingConstants.SecretMask}");
            await Assert.That(formattedResponse).DoesNotContain("response-credential");
            await Assert.That(formattedResponse).DoesNotContain("response-content-credential");
            await Assert.That(formattedResponse).Contains("X-Request-ID: response-id");
        }
    }

    [Test]
    [Arguments("fallback", false)]
    [Arguments("request-default", false)]
    [Arguments("pipeline-default", false)]
    [Arguments("request-diagnostic", true)]
    [Arguments("pipeline-diagnostic", true)]
    [Arguments("request-opt-in", true)]
    public async Task SendAsync_LogsBodiesOnlyWhenExplicitlyEnabled(string configuration, bool logBodies)
    {
        const string responseBody = "{\"access_token\":\"new-access-token\",\"token\":\"new-installation-token\"}";
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_secret"] = "unregistered-client-secret",
                ["password"] = "unregistered-password",
            }),
        };
        var requestBody = await request.Content.ReadAsStringAsync();
        using var client = new HttpClient(new TokenResponseHandler(responseBody));
        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);
        var logger = new Mock<ILogger>();
        var accessor = new Mock<IModuleLoggerAccessor>();
        accessor.SetupGet(x => x.Logger).Returns(logger.Object);
        var logging = configuration switch
        {
            "request-diagnostic" or "pipeline-diagnostic" => HttpLoggingOptions.Diagnostic,
            "request-opt-in" => new HttpLoggingOptions { ShowRequestBody = true, ShowResponseBody = true },
            _ => new HttpLoggingOptions(),
        };
        var pipelineOptions = new PipelineOptions();
        if (configuration.StartsWith("pipeline-", StringComparison.Ordinal))
        {
            pipelineOptions = pipelineOptions with { Http = pipelineOptions.Http with { Logging = logging } };
        }

        var http = new ModularPipelines.Http.Http(
            factory.Object,
            accessor.Object,
            new HttpLogger(CreateRequestFormatter(), CreateResponseFormatter()),
            Microsoft.Extensions.Options.Options.Create(pipelineOptions));
        using var response = await http.SendAsync(new HttpOptions(request)
        {
            Logging = configuration.StartsWith("request-", StringComparison.Ordinal) ? logging : null,
        });
        var logs = string.Join("\n", logger.Invocations
            .Where(invocation => invocation.Method.Name == nameof(ILogger.Log))
            .Select(invocation => invocation.Arguments[2]?.ToString()));

        using (Assert.Multiple())
        {
            await Assert.That(logs.Contains(requestBody, StringComparison.Ordinal)).IsEqualTo(logBodies);
            await Assert.That(logs.Contains(responseBody, StringComparison.Ordinal)).IsEqualTo(logBodies);
            foreach (var secret in new[] { "unregistered-client-secret", "unregistered-password", "new-access-token", "new-installation-token" })
            {
                await Assert.That(logs.Contains(secret, StringComparison.Ordinal)).IsEqualTo(logBodies);
            }
            await Assert.That(logs).Contains("POST https://example.test/token HTTP/");
            await Assert.That(logs).Contains("HTTP Response:");
            await Assert.That(logs).Contains("Content-Type:");
            await Assert.That(logs).Contains("HTTP Status:");
            await Assert.That(logs).Contains("Duration:");
            await Assert.That(await response.Content.ReadAsStringAsync()).IsEqualTo(responseBody);
        }
    }

    [Test]
    public async Task DefaultFormatters_DoNotReadBodies()
    {
        var requestStream = new CountingReadStream("client_secret=unregistered-secret"u8.ToArray());
        var responseStream = new CountingReadStream("new-token"u8.ToArray());
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/token")
        {
            Content = CreateTextContent(requestStream),
        };
        using var response = new HttpResponseMessage
        {
            Content = CreateTextContent(responseStream),
        };

        var formattedRequest = await CreateRequestFormatter().FormatAsync(request);
        var formattedResponse = await CreateResponseFormatter().FormatAsync(response);

        using (Assert.Multiple())
        {
            await Assert.That(formattedRequest).DoesNotContain("client_secret");
            await Assert.That(formattedResponse).DoesNotContain("new-token");
            await Assert.That(requestStream.BytesRead).IsEqualTo(0);
            await Assert.That(responseStream.BytesRead).IsEqualTo(0);
        }
    }

    [Test]
    [Arguments("default", false)]
    [Arguments("silent", false)]
    [Arguments("diagnostic", true)]
    [Arguments("body-opt-in", true)]
    public async Task Failure_Body_Is_Displayed_Only_In_Explicit_Logs_With_Secret_Masking(string configuration, bool logBody)
    {
        const string registeredSecret = "registered-client-secret";
        const string body = "{\"client_secret\":\"registered-client-secret\",\"access_token\":\"new-unregistered-token\"}";
        using var client = new HttpClient(new TokenResponseHandler(body, System.Net.HttpStatusCode.BadRequest));
        var logger = new Mock<ILogger>();
        var accessor = new Mock<IModuleLoggerAccessor>();
        accessor.SetupGet(x => x.Logger).Returns(logger.Object);
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            accessor.Object,
            new HttpLogger(CreateRequestFormatter(registeredSecret), CreateResponseFormatter(registeredSecret)),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test/token");
        var logging = configuration switch
        {
            "silent" => HttpLoggingOptions.Silent,
            "diagnostic" => HttpLoggingOptions.Diagnostic,
            "body-opt-in" => new HttpLoggingOptions { ShowResponseBody = true },
            _ => null,
        };

        var exception = await Assert.ThrowsAsync<PipelineHttpResponseException>(() => http.SendAsync(new HttpOptions(request)
        {
            HttpClient = client,
            Logging = logging,
        }));
        var logs = string.Join("\n", logger.Invocations
            .Where(invocation => invocation.Method.Name == nameof(ILogger.Log))
            .Select(invocation => invocation.Arguments[2]?.ToString()));

        await Assert.That(exception!.ResponseContent).IsEqualTo(body);
        await Assert.That(exception.Message).DoesNotContain("client_secret");
        await Assert.That(exception.ToString()).DoesNotContain("new-unregistered-token");
        await Assert.That(exception.ToString()).DoesNotContain(registeredSecret);
        await Assert.That(logs).DoesNotContain(registeredSecret);
        await Assert.That(logs.Contains("new-unregistered-token", StringComparison.Ordinal)).IsEqualTo(logBody);
        await Assert.That(logs.Contains($"\"client_secret\":\"{LoggingConstants.SecretMask}\"", StringComparison.Ordinal)).IsEqualTo(logBody);
    }

    private sealed class TokenResponseHandler(string body, System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
                RequestMessage = request,
            });
        }
    }

    [Test]
    public async Task ResponseFormatter_BoundsPreviewReadAndPreservesBody()
    {
        const string body = "abcdefghijklmnopqrstuvwxyz";
        var stream = new CountingReadStream(Encoding.UTF8.GetBytes(body));
        using var response = new HttpResponseMessage
        {
            Content = CreateTextContent(stream),
        };
        var formatter = CreateResponseFormatter();

        var formatted = await formatter.FormatAsync(response, new HttpLoggingOptions
        {
            ShowResponseBody = true,
            MaxBodySizeToLog = 5,
        });
        var bytesReadForPreview = stream.BytesRead;
        var replayedBody = await response.Content.ReadAsStringAsync();

        using (Assert.Multiple())
        {
            await Assert.That(bytesReadForPreview).IsLessThanOrEqualTo(6);
            await Assert.That(formatted).Contains("\tabcde");
            await Assert.That(formatted).DoesNotContain("abcdef");
            await Assert.That(replayedBody).IsEqualTo(body);
        }
    }

    [Test]
    public async Task ResponseFormatter_DoesNotAllocateEntireLargePreviewLimit()
    {
        var stream = new CountingReadStream([]);
        using var response = new HttpResponseMessage
        {
            Content = CreateTextContent(stream),
        };
        var formatter = CreateResponseFormatter();

        await formatter.FormatAsync(response, new HttpLoggingOptions
        {
            ShowResponseBody = true,
            MaxBodySizeToLog = 64 * 1024 * 1024,
        });

        await Assert.That(stream.MaximumRequestedReadSize).IsLessThanOrEqualTo(81920);
    }

    [Test]
    public async Task RequestFormatter_PreservesBodyForRepeatedSends()
    {
        const string body = "abcdefghijklmnopqrstuvwxyz";
        var stream = new CountingReadStream(Encoding.UTF8.GetBytes(body));
        var content = CreateTextContent(stream);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test")
        {
            Content = content,
        };
        var formatter = CreateRequestFormatter();

        var formatted = await formatter.FormatAsync(request, new HttpLoggingOptions
        {
            ShowRequestBody = true,
            MaxBodySizeToLog = 5,
        });
        using var firstSend = new MemoryStream();
        using var secondSend = new MemoryStream();
        await request.Content.CopyToAsync(firstSend);
        await request.Content.CopyToAsync(secondSend);

        using (Assert.Multiple())
        {
            await Assert.That(formatted).Contains("\tabcde");
            await Assert.That(formatted).DoesNotContain("abcdef");
            await Assert.That(Encoding.UTF8.GetString(firstSend.ToArray())).IsEqualTo(body);
            await Assert.That(Encoding.UTF8.GetString(secondSend.ToArray())).IsEqualTo(body);
            await Assert.That(stream.IsDisposed).IsTrue();
            await Assert.That(content.DisposeCount).IsEqualTo(1);
        }
    }

    [Test]
    public async Task ResponseFormatter_UnboundedPreviewPreservesBody()
    {
        const string body = "abcdefghijklmnopqrstuvwxyz";
        using var response = new HttpResponseMessage
        {
            Content = CreateTextContent(new MemoryStream(Encoding.UTF8.GetBytes(body))),
        };
        var formatter = CreateResponseFormatter();

        var formatted = await formatter.FormatAsync(response, new HttpLoggingOptions
        {
            ShowResponseBody = true,
            MaxBodySizeToLog = 0,
        });
        var replayedBody = await response.Content.ReadAsStringAsync();

        using (Assert.Multiple())
        {
            await Assert.That(formatted).Contains(body);
            await Assert.That(replayedBody).IsEqualTo(body);
        }
    }

    [Test]
    public async Task RequestFormatter_UnboundedPreviewPreservesBody()
    {
        const string body = "abcdefghijklmnopqrstuvwxyz";
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test")
        {
            Content = CreateTextContent(new MemoryStream(Encoding.UTF8.GetBytes(body))),
        };
        var formatter = CreateRequestFormatter();

        var formatted = await formatter.FormatAsync(request, new HttpLoggingOptions
        {
            ShowRequestBody = true,
            MaxBodySizeToLog = 0,
        });
        var replayedBody = await request.Content.ReadAsStringAsync();

        using (Assert.Multiple())
        {
            await Assert.That(formatted).Contains(body);
            await Assert.That(replayedBody).IsEqualTo(body);
        }
    }

    [Test]
    public async Task ResponseFormatter_RedactsSecretCrossingPreviewBoundary()
    {
        const string secret = "secret-value";
        using var response = new HttpResponseMessage
        {
            Content = CreateTextContent(new MemoryStream(Encoding.UTF8.GetBytes($"prefix-{secret}-suffix"))),
        };
        var formatter = CreateResponseFormatter(secret);

        var formatted = await formatter.FormatAsync(response, new HttpLoggingOptions
        {
            ShowResponseBody = true,
            MaxBodySizeToLog = 13,
        });

        using (Assert.Multiple())
        {
            await Assert.That(formatted).Contains($"prefix-{LoggingConstants.SecretMask}");
            await Assert.That(formatted).DoesNotContain("prefix-secret");
        }
    }

    [Test]
    public async Task RequestFormatter_RedactsSecretCrossingPreviewBoundary()
    {
        const string secret = "secret-value";
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test")
        {
            Content = CreateTextContent(new MemoryStream(Encoding.UTF8.GetBytes($"prefix-{secret}-suffix"))),
        };
        var formatter = CreateRequestFormatter(secret);

        var formatted = await formatter.FormatAsync(request, new HttpLoggingOptions
        {
            ShowRequestBody = true,
            MaxBodySizeToLog = 13,
        });

        using (Assert.Multiple())
        {
            await Assert.That(formatted).Contains($"prefix-{LoggingConstants.SecretMask}");
            await Assert.That(formatted).DoesNotContain("prefix-secret");
        }
    }

    [Test]
    public async Task ResponseFormatter_RedactsSecretWhenPreviewSplitsMultibyteCharacter()
    {
        const string secret = "abcédef";
        using var response = new HttpResponseMessage
        {
            Content = CreateTextContent(new MemoryStream(Encoding.UTF8.GetBytes($"prefix-{secret}-suffix"))),
        };
        var formatter = CreateResponseFormatter(secret);

        var formatted = await formatter.FormatAsync(response, new HttpLoggingOptions
        {
            ShowResponseBody = true,
            MaxBodySizeToLog = 11,
        });

        using (Assert.Multiple())
        {
            await Assert.That(formatted).Contains($"prefix-{LoggingConstants.SecretMask}");
            await Assert.That(formatted).DoesNotContain("prefix-abc");
        }
    }

    private static CountingStreamContent CreateTextContent(Stream stream)
    {
        var content = new CountingStreamContent(stream);
        content.Headers.ContentType = new MediaTypeHeaderValue("text/plain")
        {
            CharSet = "utf-8",
        };
        return content;
    }

    private sealed class CountingStreamContent(Stream stream) : StreamContent(stream)
    {
        public int DisposeCount { get; private set; }

        protected override void Dispose(bool disposing)
        {
            DisposeCount++;
            base.Dispose(disposing);
        }
    }

    private static ISecretObfuscator CreateObfuscator(params string[] secrets)
    {
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? input, object? _) => secrets.Aggregate(
                input ?? string.Empty,
                (value, secret) => value.Replace(
                    secret,
                    LoggingConstants.SecretMask,
                    StringComparison.Ordinal)));
        return obfuscator.Object;
    }

    internal static HttpRequestFormatter CreateRequestFormatter(params string[] secrets)
    {
        return new HttpRequestFormatter(
            CreateObfuscator(secrets),
            CreateSecretProvider(secrets),
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
    }

    private static HttpResponseFormatter CreateResponseFormatter(params string[] secrets)
    {
        return new HttpResponseFormatter(
            CreateObfuscator(secrets),
            CreateSecretProvider(secrets),
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
    }

    private static ISecretProvider CreateSecretProvider(string[] secrets)
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider
            .Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(0, secrets));
        return secretProvider.Object;
    }

    private sealed class CountingReadStream(byte[] contents) : MemoryStream(contents)
    {
        public int BytesRead { get; private set; }

        public int MaximumRequestedReadSize { get; private set; }

        public bool IsDisposed { get; private set; }

        public override async ValueTask<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            MaximumRequestedReadSize = Math.Max(MaximumRequestedReadSize, buffer.Length);
            var read = await base.ReadAsync(buffer, cancellationToken);
            BytesRead += read;
            return read;
        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
