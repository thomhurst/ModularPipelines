using System.Collections.Concurrent;

namespace ModularPipelines.Testing;

internal sealed class RecordingHttpHandler(
    ConcurrentQueue<HttpRequestMessage> requests,
    Func<HttpRequestMessage, CancellationToken, ValueTask<HttpResponseMessage>> handler) : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var snapshot = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy,
        };

        try
        {
            foreach (var header in request.Headers)
            {
                snapshot.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var option in request.Options)
            {
                snapshot.Options.Set(new HttpRequestOptionsKey<object?>(option.Key), option.Value);
            }

            if (request.Content is { } content)
            {
                snapshot.Content = new ByteArrayContent(
                    await content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false));
                foreach (var header in content.Headers)
                {
                    snapshot.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }
        catch
        {
            snapshot.Dispose();
            throw;
        }

        requests.Enqueue(snapshot);
        return await handler(request, cancellationToken).ConfigureAwait(false);
    }
}
