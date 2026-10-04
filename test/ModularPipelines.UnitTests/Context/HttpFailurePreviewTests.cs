using System.Net;
using System.Net.Http.Headers;
using System.Text;

using ModularPipelines.Exceptions;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class HttpFailurePreviewTests
{
    [Test]
    [Arguments("utf-8", false)]
    [Arguments("utf-8", true)]
    [Arguments("utf-16", false)]
    [Arguments("utf-16", true)]
    [Arguments("utf-16BE", false)]
    [Arguments("utf-16BE", true)]
    [Arguments("utf-32", false)]
    [Arguments("utf-32", true)]
    [Arguments("utf-32BE", false)]
    [Arguments("utf-32BE", true)]
    public async Task Failure_Preview_Decodes_And_Removes_Bom(string charset, bool declareCharset)
    {
        const string message = "Request failed: café 😀.";
        var encoding = Encoding.GetEncoding(charset);
        using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new ByteArrayContent([.. encoding.GetPreamble(), .. encoding.GetBytes(message)]),
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain")
        {
            CharSet = declareCharset ? $"\"{charset}\"" : null,
        };

        var exception = await Assert.ThrowsAsync<PipelineHttpResponseException>(() => response.EnsureSuccessStatusCodeWithContentAsync());

        await Assert.That(exception!.ResponseContent).IsEqualTo(message);
    }

    [Test]
    [Arguments("utf-8")]
    [Arguments("utf-16")]
    [Arguments("utf-16BE")]
    [Arguments("utf-32")]
    [Arguments("utf-32BE")]
    public async Task Failure_Preview_With_Bom_Does_Not_Split_Characters(string charset)
    {
        var encoding = Encoding.GetEncoding(charset);
        var message = string.Concat(Enumerable.Repeat("😀", 1000));
        using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new ByteArrayContent([.. encoding.GetPreamble(), .. encoding.GetBytes(message)]),
        };

        var exception = await Assert.ThrowsAsync<PipelineHttpResponseException>(() => response.EnsureSuccessStatusCodeWithContentAsync());

        var completeCharacters = (2000 - encoding.GetPreamble().Length) / encoding.GetByteCount("😀");
        await Assert.That(exception!.ResponseContent)
            .IsEqualTo(string.Concat(Enumerable.Repeat("😀", completeCharacters)) + "... (truncated)");
    }

    [Test]
    [Arguments(false, 7)]
    [Arguments(false, 0)]
    [Arguments(true, 7)]
    [Arguments(true, 0)]
    public async Task Preview_Preserves_Bom_In_Replayed_Content(bool replayable, int maxBytes)
    {
        byte[] bytes = [.. Encoding.Unicode.GetPreamble(), .. Encoding.Unicode.GetBytes("abcd")];
        using var content = new ByteArrayContent(bytes);

        var result = replayable
            ? await HttpContentPreviewReader.ReadReplayableAsync(content, maxBytes, CancellationToken.None)
            : await HttpContentPreviewReader.ReadAsync(content, maxBytes, CancellationToken.None);
        using var replay = result.ReplayContent;

        await Assert.That(result.Preview).IsEqualTo(maxBytes == 0 ? "abcd" : "ab");
        await Assert.That((await replay.ReadAsByteArrayAsync()).SequenceEqual(bytes)).IsTrue();
    }

    [Test]
    public async Task Failure_Preview_Prefers_Declared_Charset_Over_Bom()
    {
        byte[] bytes = [.. Encoding.Unicode.GetPreamble(), .. Encoding.Unicode.GetBytes("failure")];
        using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new ByteArrayContent(bytes),
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain") { CharSet = "iso-8859-1" };

        var exception = await Assert.ThrowsAsync<PipelineHttpResponseException>(() => response.EnsureSuccessStatusCodeWithContentAsync());

        await Assert.That(exception!.ResponseContent).IsEqualTo(Encoding.Latin1.GetString(bytes));
    }

    [Test]
    [Arguments(100)]
    [Arguments(2000)]
    [Arguments(2001)]
    [Arguments(10_000_000)]
    [Arguments(-1)]
    public async Task Failure_Reads_Only_A_Bounded_Preview(int length)
    {
        using var stream = new PreviewStream(length);
        using var client = new HttpClient(new ResponseHandler(new StreamContent(stream)));
        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(value => value.CreateClient(It.IsAny<string>())).Returns(client);
        var http = new ModularPipelines.Http.Http(factory.Object, Mock.Of<IModuleLoggerAccessor>(),
            Mock.Of<IHttpLogger>(), Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test/failure");

        var exception = await Assert.ThrowsAsync<PipelineHttpResponseException>(() => http.SendAsync(new HttpOptions(request)));
        var truncated = length < 0 || length > 2000;
        await Assert.That(exception!.ResponseContent).IsEqualTo(new string('x', truncated ? 2000 : length) + (truncated ? "... (truncated)" : string.Empty));
        await Assert.That(stream.ReadPastLimit).IsFalse();
        await Assert.That(stream.BytesRead).IsEqualTo(length < 0 ? 2001 : Math.Min(length, 2001));
        await Assert.That(stream.Disposed).IsTrue();
    }

    [Test]
    public async Task Failure_Preview_Does_Not_Split_Multibyte_Characters()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(new string('€', 1000), System.Text.Encoding.UTF8),
        };
        var exception = await Assert.ThrowsAsync<PipelineHttpResponseException>(() => response.EnsureSuccessStatusCodeWithContentAsync());
        await Assert.That(exception!.ResponseContent).IsEqualTo(new string('€', 666) + "... (truncated)");
    }

    private sealed class ResponseHandler(HttpContent content) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = content });
    }

    private sealed class PreviewStream(int length) : Stream
    {
        public int BytesRead { get; private set; }
        public bool ReadPastLimit { get; private set; }
        public bool Disposed { get; private set; }
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => BytesRead; set => throw new NotSupportedException(); }
        public override int Read(byte[] buffer, int offset, int count) => Read(buffer.AsSpan(offset, count));
        public override int Read(Span<byte> buffer)
        {
            var count = length < 0 ? buffer.Length : Math.Min(buffer.Length, length - BytesRead);
            if (BytesRead + count > 2001)
            {
                ReadPastLimit = true;
                throw new InvalidOperationException("Failure response must not be drained beyond its preview.");
            }

            buffer[..count].Fill((byte) 'x');
            BytesRead += count;
            return count;
        }
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ValueTask.FromResult(Read(buffer.Span));
        }
        protected override void Dispose(bool disposing)
        {
            Disposed = true;
            base.Dispose(disposing);
        }
        public override void Flush() => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
