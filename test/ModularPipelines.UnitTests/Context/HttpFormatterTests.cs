using ModularPipelines.Secrets;
using System.Net.Http.Headers;
using System.Text;
using ModularPipelines.Constants;
using ModularPipelines.Engine;
using ModularPipelines.Http;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class HttpFormatterTests
{
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

    private static HttpRequestFormatter CreateRequestFormatter(params string[] secrets)
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
