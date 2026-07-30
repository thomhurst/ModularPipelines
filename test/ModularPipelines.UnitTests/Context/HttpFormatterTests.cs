using System.Net.Http.Headers;
using System.Text;
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
        var formatter = new HttpResponseFormatter(CreateObfuscator());

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
    public async Task RequestFormatter_BoundsPreviewReadAndPreservesBody()
    {
        const string body = "abcdefghijklmnopqrstuvwxyz";
        var stream = new CountingReadStream(Encoding.UTF8.GetBytes(body));
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test")
        {
            Content = CreateTextContent(stream),
        };
        var formatter = new HttpRequestFormatter(CreateObfuscator());

        var formatted = await formatter.FormatAsync(request, new HttpLoggingOptions
        {
            MaxBodySizeToLog = 5,
        });
        var bytesReadForPreview = stream.BytesRead;
        var replayedBody = await request.Content.ReadAsStringAsync();

        using (Assert.Multiple())
        {
            await Assert.That(bytesReadForPreview).IsLessThanOrEqualTo(6);
            await Assert.That(formatted).Contains("\tabcde");
            await Assert.That(formatted).DoesNotContain("abcdef");
            await Assert.That(replayedBody).IsEqualTo(body);
        }
    }

    private static StreamContent CreateTextContent(Stream stream)
    {
        var content = new StreamContent(stream);
        content.Headers.ContentType = new MediaTypeHeaderValue("text/plain")
        {
            CharSet = "utf-8",
        };
        return content;
    }

    private static ISecretObfuscator CreateObfuscator()
    {
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? input, object? _) => input ?? string.Empty);
        return obfuscator.Object;
    }

    private sealed class CountingReadStream(byte[] contents) : MemoryStream(contents)
    {
        public int BytesRead { get; private set; }

        public override async ValueTask<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            var read = await base.ReadAsync(buffer, cancellationToken);
            BytesRead += read;
            return read;
        }
    }
}
