using System.Net;

using ModularPipelines.Exceptions;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class HttpFailurePreviewTests
{
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
